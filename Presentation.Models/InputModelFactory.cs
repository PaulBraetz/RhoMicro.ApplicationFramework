namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for producing instances of <see cref="InputModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public sealed class InputModelFactory<TValue, TError> : IFactory<IInputModel<TValue, TError>>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="valueDefaultProvider">The default error value provider to initialize newly created models with.</param>
    /// <param name="errorDefaultProvider">The default value provider to initialize newly created models with.</param>
    public InputModelFactory(IDefaultValueProvider<TValue> valueDefaultProvider, IDefaultValueProvider<TError> errorDefaultProvider)
    {
        _valueDefaultProvider = valueDefaultProvider;
        _errorDefaultProvider = errorDefaultProvider;
    }

    private readonly IDefaultValueProvider<TValue> _valueDefaultProvider;
    private readonly IDefaultValueProvider<TError> _errorDefaultProvider;

    /// <inheritdoc/>
    public IInputModel<TValue, TError> Create() =>
        new InputModel<TValue, TError>(_valueDefaultProvider, _errorDefaultProvider);
}
