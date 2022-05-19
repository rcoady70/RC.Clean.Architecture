using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Exceptions;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.Constants;
using RC.CA.Application.Models;
using RC.CA.SharedKernel.Extensions;
using System.Text;

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
    public async Task<TOut> SendAsync<TIn, TOut>(TIn request, string endPoint, HttpMethod method)
           where TOut : BaseResponseDto, new() //Output object must inherit from BaseResponseDto 
    {
        //Create empty response object 
        TOut? response = new TOut();

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
            response = await ProssessResponse<TIn, TOut>(apiResult, endPoint, request);
        }
        catch (WebException ex)
        {
            throw new ApiException(_appContext.CorrelationId, endPoint, $"HttpPostHelper WebException {ex.Message}", (int)ex.Status, JsonSerializer.Serialize(request).MaskSensitiveDataExt(), JsonSerializer.Serialize(response).MaskSensitiveDataExt(), ex);
        }
        catch (JsonException ex)
        {
            throw new ApiException(_appContext.CorrelationId, endPoint, $"HttpPostHelper JsonException {ex.Message}", 0, JsonSerializer.Serialize(request).MaskSensitiveDataExt(), JsonSerializer.Serialize(response).MaskSensitiveDataExt(), ex);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(LoggerEvents.APIEvt, ex, @"API failed: Request-Id {CorrelationId} Endpoint {endPoint} Error {errormessage}  Request: {request} ", endPoint, ex.Message, JsonSerializer.Serialize(response).MaskSensitiveDataExt(), _appContext.CorrelationId);
            ex.Data.Add("ApiEndpoint", endPoint);
            throw; //Use throw it will preserve stack trace
        }
        return response;
    }
    
     /// <summary>
    /// Process api response
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <param name="apiResult"></param>
    /// <param name="endPoint"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    private async Task<TOut> ProssessResponse<TIn,TOut>(HttpResponseMessage apiResult,string endPoint,TIn request)
                                                    where TOut : BaseResponseDto, new() //Output object must inherit from BaseResponseDto 
    {
        TOut? response = new TOut();
        string strResponse = "";
        switch (apiResult.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.BadRequest:
                strResponse = await apiResult.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(strResponse))
                    response = JsonSerializer.Deserialize<TOut>(strResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                else
                {
                    response = new TOut();
                    await response.AddResponseError(BaseResponseDto.ErrorType.Error, $"No response object form endpoint {endPoint}");
                    _logger.LogError(LoggerEvents.APIEvt, @"API return object de-serialize failed: Request-Id {requestId} Endpoint {endPoint}  Request: {request} ", endPoint, JsonSerializer.Serialize(request).MaskSensitiveDataExt(), _appContext.CorrelationId);
                }
                break;
            case HttpStatusCode.Unauthorized:
                response.RequestStatus = HttpStatusCode.Unauthorized;
                await response.AddResponseError(BaseResponseDto.ErrorType.Unauthorized, $"Unauthorized {endPoint}");
                break;
            default:
                strResponse = await apiResult.Content.ReadAsStringAsync();
                var baseResponse = JsonSerializer.Deserialize<BaseResponseDto>(strResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new ApiException(_appContext.CorrelationId, endPoint, $"HttpPostHelper WebException {baseResponse?.Errors?[0]}", (int)baseResponse.RequestStatus, JsonSerializer.Serialize(request).MaskSensitiveDataExt(), JsonSerializer.Serialize(baseResponse).MaskSensitiveDataExt(), null);
                break;
        }
        return response;
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
