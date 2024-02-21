namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for instances of <see cref="SelectInputModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public sealed class SelectInputModelFactory<TValue, TError> : IFactory<ISelectInputModel<TValue, TError>>
{
    private readonly IDefaultValueProviderFactory<IOptionModel<TValue>?> _valueDefaultProviderFactory;
    private readonly IDefaultValueProvider<TError> _errorDefaultProvider;
    private readonly IOptionsProvider<TValue> _optionsProvider;
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="optionsProvider">The provider to use when requesting the options results may present.</param>
    /// <param name="valueDefaultProviderFactory">The default value provider to use for initializing <see cref="SelectInputModel{TValue, TError}.Value"/>.</param>
    /// <param name="errorDefaultProvider">The default value provider to use for initializing <see cref="InputModel{TValue, TError}.ErrorValue"/>.</param>
    public SelectInputModelFactory(
        IOptionsProvider<TValue> optionsProvider,
        IDefaultValueProviderFactory<IOptionModel<TValue>?> valueDefaultProviderFactory,
        IDefaultValueProvider<TError> errorDefaultProvider)
    {
        _valueDefaultProviderFactory = valueDefaultProviderFactory;
        _errorDefaultProvider = errorDefaultProvider;
        _optionsProvider = optionsProvider;
    }

    /// <inheritdoc/>
    public ISelectInputModel<TValue, TError> Create()
    {
        var options = _optionsProvider.GetOptions();
        var result = SelectInputModel<TValue, TError>.Create(options, _valueDefaultProviderFactory, _errorDefaultProvider);

        return result;
    }
}
