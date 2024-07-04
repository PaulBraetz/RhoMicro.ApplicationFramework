namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for producing instances of <see cref="InputGroupModel{TInput, TValue, TError}"/>.
/// </summary>
/// <typeparam name="TInput">The type of input used to obtain values.</typeparam>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="inputFactory">The factory to use when initializing the input model for results.</param>
public sealed class InputGroupModelFactory<TInput, TValue, TError>(IFactory<TInput> inputFactory)
    : IFactory<IInputGroupModel<TInput, TValue, TError>>
    where TInput : IInputModel<TValue, TError>
{
    /// <inheritdoc/>
    public IInputGroupModel<TInput, TValue, TError> Create()
    {
        var input = inputFactory.Create();
        var result = new InputGroupModel<TInput, TValue, TError>(input);

        return result;
    }
}
