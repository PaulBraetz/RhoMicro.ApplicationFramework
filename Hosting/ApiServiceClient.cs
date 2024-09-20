namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;

sealed class ApiServiceClient<TRequest, TResult, TRequestDto, TResultDto>(
    IHttpClientFactory clientFactory, ApiServiceClientSettings<TRequest, TResult> settings)
    : IService<TRequest, TResult>
    where TRequest : IApiRequest<TRequest, TResult, TRequestDto, TResultDto>, IRequest<TResult>
    where TResult : IApiResult<TResult, TResultDto>
    where TRequestDto : IApiRequestDto<TRequest, TResult>
    where TResultDto : IApiResultDto<TResult>
{
    public async ValueTask<TResult> Execute(TRequest request, CancellationToken cancellationToken)
    {
        var client = clientFactory.CreateClient(GetType().FullName!);
        var requestDto = request.ToDto();
        using var httpResponse = await client.PostAsJsonAsync(
            settings.RequestUri,
            requestDto,
            settings.SerializerOptions,
            cancellationToken);
        TResultDto? resultDto;
        try
        {
            resultDto = await httpResponse.Content.ReadFromJsonAsync<TResultDto>(settings.SerializerOptions, cancellationToken);
        } catch(Exception ex)
        {
            throw new ApiServiceDeserializationException(typeof(TResultDto), ex);
        }

        if(resultDto is null)
            throw new ApiServiceDeserializationException(typeof(TResultDto), await httpResponse.Content.ReadAsStringAsync(cancellationToken));

        var result = resultDto.ToResult();

        return result;
    }
}
