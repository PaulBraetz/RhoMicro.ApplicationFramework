#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace TA.Presentation.Views.Blazor;

using System;

using RhoMicro.ApplicationFramework.Common.Abstractions;

public partial record struct ToUpper :
    IApiRequest<ToUpper, ToUpper.Result, ToUpper, ToUpper.Result>,
    IApiRequestDto<ToUpper, ToUpper.Result>
{
    public readonly partial record struct Result(String Value)
        : IApiResult<Result, Result>,
        IApiResultDto<Result>
    {
        public Result ToResult() => this;
        public Result ToDto() => this;
    }
    public ToUpper ToDto() => this;
    public ToUpper ToRequest() => this;
}
