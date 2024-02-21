namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Provides default values for types. Intended to provide initial values for models like the input model.
/// </summary>
/// <typeparam name="T">The type for which to provide a default value.</typeparam>
public interface IDefaultValueProvider<T>
{
    /// <summary>
    /// Gets a default value for <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The default value for <typeparamref name="T"/>.</returns>
    T GetDefault();
}