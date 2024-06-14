#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace TA.Presentation.Views.Blazor;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;

public readonly record struct ToLowerRequest(
      String Value, [property: JsonIgnore] CancellationToken CancellationToken)
    : IServiceRequest<ToLowerRequest.Result>,
      IApiServiceRequest<ToLowerRequest, ToLowerRequest.Result, ToLowerRequest, ToLowerRequest.Result>,
      IApiServiceRequestDto<ToLowerRequest, ToLowerRequest.Result>
{
    public ToLowerRequest ToDto() => this;
    public ToLowerRequest ToRequest(CancellationToken cancellationToken) => this;

    public readonly record struct Result(String Value)
        : IApiServiceResult<Result, Result>, IApiServiceResultDto<Result>
    {
        public Result ToDto() => this;
        public Result ToResult() => this;
    }
}

public sealed class ToLowerService : IService<ToLowerRequest, ToLowerRequest.Result>
{
    public ValueTask<ToLowerRequest.Result> Execute(ToLowerRequest request) =>
        ValueTask.FromResult(new ToLowerRequest.Result(request.Value.ToLowerInvariant()));
}