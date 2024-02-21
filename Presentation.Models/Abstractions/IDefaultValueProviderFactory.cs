namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for instances of <see cref="IDefaultValueProvider{T}"/>.
/// </summary>
/// <typeparam name="T">The type for which to provide a default value.</typeparam>
public interface IDefaultValueProviderFactory<T>
{
    /// <summary>
    /// Creates a new default value provider using the factory provided.
    /// </summary>
    /// <param name="defaultValueFcatory">The factory the resulting provider should invoke upon a default value being requested.</param>
    /// <returns>A new instance of <see cref="IDefaultValueProvider{T}"/>, based on the value factory provided.</returns>
    IDefaultValueProvider<T> Create(Func<T> defaultValueFcatory);
}