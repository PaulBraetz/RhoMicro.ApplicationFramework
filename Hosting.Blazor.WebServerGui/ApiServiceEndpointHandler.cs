namespace RhoMicro.ApplicationFramework.Hosting;

using System;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

sealed class ApiServiceEndpointHandler<TRequest, TResult, TRequestDto, TResultDto>
    (IService<TRequest, TResult> service, ApiServiceEndpointHandlerSettings settings)
    where TRequest : IApiRequest<TRequest, TResult, TRequestDto, TResultDto>, IRequest<TResult>
    where TResult : IApiResult<TResult, TResultDto>
    where TRequestDto : IApiRequestDto<TRequest, TResult>
    where TResultDto : IApiResultDto<TResult>
{
    public async Task Handle(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var requestDto = await JsonSerializer.DeserializeAsync<TRequestDto>(context.Request.Body, settings.SerializerOptions, context.RequestAborted)
            ?? throw new InvalidOperationException("Unable to deserialize request dto.");

        var request = requestDto.ToRequest();
        var result = await service.Execute(request, context.RequestAborted);
        var resultDto = result.ToDto();
        await JsonSerializer.SerializeAsync(context.Response.Body, resultDto, settings.SerializerOptions, context.RequestAborted);
    }
}
