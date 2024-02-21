namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Provides default values for types. Intended to provide initial values for models like the input model.
/// </summary>
/// <typeparam name="T">The type for which to provide a default value.</typeparam>
public sealed class DefaultValueProvider<T> : IDefaultValueProvider<T>
{
    /// <summary>
    /// The provider used when invoking <see cref="GetDefault"/>. The default provider will provide the default value for <typeparamref name="T"/>.
    /// </summary>
    /// <param name="provider"></param>
    public DefaultValueProvider(Func<T> provider)
    {
        _provider = provider;
    }
    /// <summary>
    /// Gets or sets the provider used when invoking <see cref="GetDefault"/>. The default provider will provide the default value for <typeparamref name="T"/>.
    /// </summary>
    private readonly Func<T> _provider;

    /// <summary>
    /// Gets a default value for <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The default value for <typeparamref name="T"/>.</returns>
    public T GetDefault()
    {
        var result = _provider.Invoke();

        return result;
    }
}
