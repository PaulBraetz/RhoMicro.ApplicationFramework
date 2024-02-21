namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for producing instances of <see cref="InputGroupModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public sealed class InputGroupModelFactory<TValue, TError> : IFactory<IInputGroupModel<TValue, TError>>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="inputFactory">The factory to use when initializing the input model for results.</param>
    public InputGroupModelFactory(IFactory<IInputModel<TValue, TError>> inputFactory)
    {
        _inputFactory = inputFactory;
    }

    private readonly IFactory<IInputModel<TValue, TError>> _inputFactory;

    /// <inheritdoc/>
    public IInputGroupModel<TValue, TError> Create()
    {
        var input = _inputFactory.Create();
        var result = new InputGroupModel<TValue, TError>(input);

        return result;
    }
}
