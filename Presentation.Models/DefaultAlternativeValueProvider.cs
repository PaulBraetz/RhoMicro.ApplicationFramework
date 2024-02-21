namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Implementation of <see cref="IDefaultValueProvider{T}"/> that returns the first value from a set of values, or a value obtained from a fallback provider if the set is empty.
/// </summary>
/// <typeparam name="T">The type for which to provide a default value.</typeparam>
public sealed class DefaultAlternativeValueProvider<T> : IDefaultValueProvider<T>
{
    private readonly IDefaultValueProvider<T> _fallbackProvider;
    private readonly ISet<T> _values;
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="fallbackProvider">The provider to use in case the set of available values is empty.</param>
    /// <param name="values">The set of values from which to return the first item.</param>
    public DefaultAlternativeValueProvider(IDefaultValueProvider<T> fallbackProvider, ISet<T> values)
    {
        _fallbackProvider = fallbackProvider;
        _values = values;
    }

    /// <inheritdoc/>
    public T GetDefault()
    {
        var result = _values.Count > 0 ?
            _values.First() :
            _fallbackProvider.GetDefault();

        return result;
    }
}
