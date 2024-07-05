#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace TA.Presentation.Views.Blazor;

using System;

using RhoMicro.ApplicationFramework.Aspects;
using RhoMicro.ApplicationFramework.Common.Abstractions;

public partial record struct ToLower :
    IApiRequest<ToLower, ToLower.Result, ToLower, ToLower.Result>,
    IApiRequestDto<ToLower, ToLower.Result>
{
    public readonly partial record struct Result(String Value)
        : IApiResult<Result, Result>,
        IApiResultDto<Result>
    {
        public Result ToResult() => this;
        public Result ToDto() => this;
    }
    public ToLower ToDto() => this;
    public ToLower ToRequest() => this;
}

public sealed partial class ToLowerService
{
    [ServiceMethod(Namespace = "", RequestTypeKind = RequestTypeKind.RecordStruct, Visibility = ServiceVisibility.Public)]
    public async ValueTask<ToLower.Result> ToLower(String value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);

        await Task.Delay(( new[] { 10, 100, 1500, 2500 } )[Random.Shared.Next(0, 4)], cancellationToken);

        return new(value.ToLowerInvariant());
    }
}