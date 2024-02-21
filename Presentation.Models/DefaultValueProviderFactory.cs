namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for producing instances of <see cref="DefaultValueProvider{T}"/>.
/// </summary>
/// <typeparam name="T">The type for which to provide a default value.</typeparam>
public sealed class DefaultValueProviderFactory<T> : IDefaultValueProviderFactory<T>
{
    /// <inheritdoc/>
    public IDefaultValueProvider<T> Create(Func<T> defaultValueFcatory) => new DefaultValueProvider<T>(defaultValueFcatory);
}
