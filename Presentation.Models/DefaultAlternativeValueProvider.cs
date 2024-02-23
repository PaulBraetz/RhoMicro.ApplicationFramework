namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Implementation of <see cref="IDefaultValueProvider{T}"/> that returns the first value from a set of values, or a value obtained from a fallback provider if the set is empty.
/// </summary>
/// <typeparam name="T">The type for which to provide a default value.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="fallbackProvider">The provider to use in case the set of available values is empty.</param>
/// <param name="values">The set of values from which to return the first item.</param>
public sealed class DefaultAlternativeValueProvider<T>(IDefaultValueProvider<T> fallbackProvider, ISet<T> values) : IDefaultValueProvider<T>
{
    /// <inheritdoc/>
    public T GetDefault()
    {
        var result = values.Count > 0 ?
            values.First() :
            fallbackProvider.GetDefault();

        return result;
    }
}
