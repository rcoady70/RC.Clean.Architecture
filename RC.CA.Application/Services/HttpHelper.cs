﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Exceptions;
using RC.CA.Application.Models;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.Models.FluentError;

namespace RC.CA.Application.Services;

/// <summary>
/// Return parameters have to be inherited from base response
/// </summary>
/// <typeparam name="TIn">Request object</typeparam>
/// <typeparam name="TOut">Response object from api call</typeparam>
public class HttpHelper : IHttpHelper
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpHelper> _logger;
    private readonly IAppContextX _appContext;
    private HttpClient _httpClient;
    private string _apiResultAsString = "";
    /// <summary>
    /// Http client helper. Facilitate interaction with api
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public HttpHelper(IHttpClientFactory httpClientFactory, ILogger<HttpHelper> logger,
                      IAppContextX appContext)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _appContext = appContext;
    }
    /// <summary>
    /// HTTP client helper. TIn Request object return response object TOut
    /// </summary>
    /// <param name="request">Request object sent to </param>
    /// <param name="endPoint">Endpoint</param>
    /// <param name="method">Method put,post,get,patch</param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public async Task<CAResult<TOut>> SendAsync<TIn, TOut>(TIn request, string endPoint, HttpMethod method)
        where TOut : BaseResponseDto, new()
        where TIn : IServiceRequest
    {

        _httpClient = _httpClientFactory.CreateClient(RC.CA.SharedKernel.Constants.WebConstants.HttpFactoryName);

        //Create request message
        HttpRequestMessage message = new HttpRequestMessage();
        message.Headers.Add("Accept", "application/json");
        message.RequestUri = new Uri($"{_httpClient.BaseAddress}{endPoint}");
        message.Method = method;

        if (request != null)
            message.Content = new StringContent(request.ToJsonExt(), Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        await SetRequestHeaders();

        try
        {
            var apiResult = await _httpClient.SendAsync(message);
            return await ProssessResponse<TIn, TOut>(apiResult, endPoint, request);
        }
        catch (WebException ex)
        {
            throw new ApiException(_appContext.CorrelationId, endPoint, $"HttpPostHelper WebException {ex.Message}", (int)ex.Status,
                                   JsonSerializer.Serialize(request).MaskSensitiveDataExt(),
                                   "", ex);
        }
        catch (JsonException ex)
        {
            throw new ApiException(_appContext.CorrelationId, endPoint, $"HttpPostHelper JsonException {ex.Message}", 0,
                                    JsonSerializer.Serialize(request).MaskSensitiveDataExt(),
                                    _apiResultAsString.MaskSensitiveDataExt(), ex);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(LoggerEvents.APIEvt, ex, @"API failed: Request-Id {CorrelationId} Endpoint {endPoint} Error {errormessage}  Request: {request} ",
                             endPoint,
                             ex.Message,
                             "", _appContext.CorrelationId);
            ex.Data.Add("ApiEndpoint", endPoint);
            throw; //Use throw it will preserve stack trace
        }
    }
    private async Task<CAResult<TOut>> ProssessResponse<TIn, TOut>(HttpResponseMessage apiResult, string endPoint, TIn request)
                                                                where TOut : BaseResponseDto, new()
    {
        var dftResponse = CAResult<TOut>.Invalid(new ValidationError
        {
            ErrorCode = "RSP0000",
            ErrorMessage = $"Failed to process response type of {typeof(TOut).Name}",
            Identifier = "",
            Severity = ValidationSeverity.Error
        });

        switch (apiResult.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.BadRequest:
                _apiResultAsString = await apiResult.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(_apiResultAsString))
                {
                    //catch fluent validation errors which may be returned.
                    if (_apiResultAsString.IndexOf("https://tools.ietf.org/html/rfc7231#section-6.5.1") > 0)
                    {
                        var fluentResponse = JsonSerializer.Deserialize<FluentErrorModel>(_apiResultAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        foreach (var fError in fluentResponse?.errors)
                        {
                            foreach (var error in fError.Value)
                                dftResponse.ValidationErrors.Add(new ValidationError
                                {
                                    ErrorCode = "",
                                    ErrorMessage = error,
                                    Identifier = "",
                                    Severity = ValidationSeverity.Error
                                });
                        }
                    }
                    else
                        return JsonSerializer.Deserialize<CAResult<TOut>>(_apiResultAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    dftResponse.ValidationErrors.Add(new ValidationError
                    {
                        ErrorCode = "RSP0001",
                        ErrorMessage = $"No response object form endpoint {endPoint}",
                        Identifier = "",
                        Severity = ValidationSeverity.Error
                    });
                    _logger.LogError(LoggerEvents.APIEvt, @"API return object de-serialize failed: Request-Id {requestId} Endpoint {endPoint}  Request: {request} ", endPoint, JsonSerializer.Serialize(request).MaskSensitiveDataExt(), _appContext.CorrelationId);
                }
                break;
            case HttpStatusCode.Unauthorized:
                var unAuthResponse = CAResult<TOut>.Unauthorized();
                unAuthResponse.ValidationErrors.Add(new ValidationError
                {
                    ErrorCode = "RSP0002",
                    ErrorMessage = $"Unauthorized {endPoint}",
                    Identifier = "",
                    Severity = ValidationSeverity.Error
                });
                return unAuthResponse;
                break;
            default:
                _apiResultAsString = await apiResult.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<CAResult<TOut>>(_apiResultAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiException(_appContext.CorrelationId, endPoint, $"HttpPostHelper WebException {response?.ValidationErrors?[0]}", (int)response.Status, JsonSerializer.Serialize(request).MaskSensitiveDataExt(), JsonSerializer.Serialize(response).MaskSensitiveDataExt(), null);
                break;
        }
        return dftResponse;
    }

    /// <summary>
    /// Set default headers. Bearer token & Correlation Id to tie requests together between from and back end
    /// </summary>
    private async Task SetRequestHeaders()
    {
        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing API request {nameof(SendAsync)} CID {_appContext.CorrelationId}");

        //Set authentication bearer token
        if (!string.IsNullOrEmpty(_appContext.JwtToken))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _appContext.JwtToken);

        //Add CorrelationId header. Is created in EnrichLoggingMiddleware middleware
        _httpClient.DefaultRequestHeaders.Add(WebConstants.CorrelationId, _appContext.CorrelationId);

        //Add IP address to forward orginal ip
        _httpClient.DefaultRequestHeaders.Add("X-Forwarded-For", _appContext.IpAddress);
    }
}
