namespace RhoMicro.ApplicationFramework.Hosting;

using System;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

sealed class ApiServiceEndpointHandler<TRequest, TResult, TRequestDto, TResultDto>
    (IService<TRequest, TResult> service, ApiServiceEndpointHandlerSettings settings)
    where TRequest : IApiServiceRequest<TRequest, TResult, TRequestDto, TResultDto>, IServiceRequest<TResult>
    where TResult : IApiServiceResult<TResult, TResultDto>
    where TRequestDto : IApiServiceRequestDto<TRequest, TResult>
    where TResultDto : IApiServiceResultDto<TResult>
{
    public async Task Handle(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var requestDto = await JsonSerializer.DeserializeAsync<TRequestDto>(context.Request.Body, settings.SerializerOptions)
            ?? throw new InvalidOperationException("Unable to deserialize request dto.");

        var ct = context.RequestAborted;
        var request = requestDto.ToRequest(ct);
        var result = await service.Execute(request).ConfigureAwait(false);
        var resultDto = result.ToDto();
        await JsonSerializer.SerializeAsync(context.Response.Body, resultDto, settings.SerializerOptions, ct).ConfigureAwait(false);
    }
}
