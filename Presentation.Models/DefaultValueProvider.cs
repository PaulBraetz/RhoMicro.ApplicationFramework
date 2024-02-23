namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Provides default values for types. Intended to provide initial values for models like the input model.
/// </summary>
/// <typeparam name="T">The type for which to provide a default value.</typeparam>
/// <remarks>
/// The provider used when invoking <see cref="GetDefault"/>. The default provider will provide the default value for <typeparamref name="T"/>.
/// </remarks>
/// <param name="provider"></param>
public sealed class DefaultValueProvider<T>(Func<T> provider) : IDefaultValueProvider<T>
{
    /// <summary>
    /// Gets a default value for <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The default value for <typeparamref name="T"/>.</returns>
    public T GetDefault()
    {
        var result = provider.Invoke();

        return result;
    }
}
