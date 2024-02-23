namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for instances of <see cref="SelectInputModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="optionsProvider">The provider to use when requesting the options results may present.</param>
/// <param name="valueDefaultProviderFactory">The default value provider to use for initializing <see cref="SelectInputModel{TValue, TError}.Value"/>.</param>
/// <param name="errorDefaultProvider">The default value provider to use for initializing <see cref="InputModel{TValue, TError}.ErrorValue"/>.</param>
public sealed class SelectInputModelFactory<TValue, TError>(
    IOptionsProvider<TValue> optionsProvider,
    IDefaultValueProviderFactory<IOptionModel<TValue>?> valueDefaultProviderFactory,
    IDefaultValueProvider<TError> errorDefaultProvider) : IFactory<ISelectInputModel<TValue, TError>>
{
    /// <inheritdoc/>
    public ISelectInputModel<TValue, TError> Create()
    {
        var options = optionsProvider.GetOptions();
        var result = SelectInputModel.Create(options, valueDefaultProviderFactory, errorDefaultProvider);

        return result;
    }
}
