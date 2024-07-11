#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace TA.Presentation.Views.Blazor;

using System;

using RhoMicro.ApplicationFramework.Aspects;

public sealed partial class ToLowerService
{
    [ServiceMethod]
    public async ValueTask<ToLower.Result> ToLower(String value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);

        await Task.Delay(( new[] { 10, 100, 1500, 2500 } )[Random.Shared.Next(0, 4)], cancellationToken);

        return new(value.ToLowerInvariant());
    }
    [ServiceMethod]
    public async ValueTask<ToLower.Result> ToUpper(String value, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(value);

        await Task.Delay(( new[] { 10, 100, 1500, 2500 } )[Random.Shared.Next(0, 4)], ct);

        return new(value.ToLowerInvariant());
    }
}