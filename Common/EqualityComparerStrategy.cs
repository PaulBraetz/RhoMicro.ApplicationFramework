namespace RhoMicro.ApplicationFramework.Common;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Strategy based implementatio of <see cref="IEqualityComparer{T}"/>.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="equalityStrategy">The strategy to use when invoking <see cref="Equals(T?, T?)"/>.</param>
/// <param name="hashCodeStrategy">The strategy to use when invoking <see cref="GetHashCode(T)"/>.</param>
public sealed class EqualityComparerStrategy<T>(Func<T, T, Boolean> equalityStrategy, Func<T, Int32> hashCodeStrategy) : IEqualityComparer<T>
{
    /// <inheritdoc/>
    public Boolean Equals(T? x, T? y)
    {
        var result = x == null ? y == null : y != null && equalityStrategy(x, y);

        return result;
    }

    /// <inheritdoc/>
    public Int32 GetHashCode([DisallowNull] T obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return hashCodeStrategy(obj);
    }
}
