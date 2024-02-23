namespace RhoMicro.ApplicationFramework.Presentation.Models;
using System.Diagnostics.CodeAnalysis;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Implementation of <see cref="IEqualityComparer{T}"/> for <see cref="IToastModel"/>, based on <see cref="IToastModel.CreatedAt"/>, <see cref="IToastModel.Header"/> and <see cref="IToastModel.Body"/>.
/// </summary>
public sealed class ToastModelEqualityComparer : IEqualityComparer<IToastModel>
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
    public static ToastModelEqualityComparer Instance { get; } = new();
    /// <inheritdoc/>
    public Boolean Equals(IToastModel? x, IToastModel? y)
    {
        var result = x == null
                    ? y == null
                    : y != null
                    && x.CreatedAt == y.CreatedAt
                    && x.Header == y.Header
                    && x.Body == y.Body;

        return result;
    }
    /// <inheritdoc/>
    public Int32 GetHashCode([DisallowNull] IToastModel obj)
    {
        _ = obj ?? throw new ArgumentNullException(nameof(obj));

        var result = HashCode.Combine(obj.CreatedAt, obj.Header, obj.Body);

        return result;
    }
}

