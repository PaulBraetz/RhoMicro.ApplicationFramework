namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;

sealed class ApiServiceClient<TRequest, TResult, TRequestDto, TResultDto>(
    IHttpClientFactory clientFactory, ApiServiceClientSettings<TRequest, TResult> settings)
    : IService<TRequest, TResult>
    where TRequest : IApiServiceRequest<TRequest, TResult, TRequestDto, TResultDto>, IServiceRequest<TResult>
    where TResult : IApiServiceResult<TResult, TResultDto>
    where TRequestDto : IApiServiceRequestDto<TRequest, TResult>
    where TResultDto : IApiServiceResultDto<TResult>
{
    public async ValueTask<TResult> Execute(TRequest request)
    {
        var client = clientFactory.CreateClient(GetType().FullName!);
        var requestDto = request.ToDto();
        using var httpResponse = await client.PostAsJsonAsync(
            settings.RequestUri,
            requestDto,
            settings.SerializerOptions,
            request.CancellationToken).ConfigureAwait(false);
        TResultDto? resultDto;
        try
        {
            resultDto = await httpResponse.Content.ReadFromJsonAsync<TResultDto>(settings.SerializerOptions, request.CancellationToken).ConfigureAwait(false);
        } catch(Exception ex)
        {
            throw new ApiServiceDeserializationException(typeof(TResultDto), ex);
        }

        if(resultDto is null)
            throw new ApiServiceDeserializationException(typeof(TResultDto), await httpResponse.Content.ReadAsStringAsync(request.CancellationToken).ConfigureAwait(false));

        var result = resultDto.ToResult();

        return result;
    }
}
