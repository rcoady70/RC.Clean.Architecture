﻿using RC.CA.Application.Models;

namespace RC.CA.Application.Contracts.Services;

public interface IHttpHelper
{
    /// <summary>
    /// Http client helper. TIn Request object return response object TOut
    /// </summary>
    /// <param name="request">Request object sent to </param>
    /// <param name="endPoint">Endpoint</param>
    /// <param name="method">Method put,post,get,patch</param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>

    Task<CAResult<TOut>> SendAsync<TIn, TOut>(TIn request, string endPoint, HttpMethod method)
        where TOut : BaseResponseDto, new()
        //Mist inherit from marker interface IServiceRequest
        where TIn : IServiceRequest;

}
