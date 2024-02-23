namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for producing instances of <see cref="SelectInputGroupModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="inputModelFactory">The factory to use when producing the input model for results.</param>
public sealed class SelectInputGroupModelFactory<TValue, TError>(IFactory<ISelectInputModel<TValue, TError>> inputModelFactory)
    : IFactory<ISelectInputGroupModel<TValue, TError>>
{
    /// <inheritdoc/>
    public ISelectInputGroupModel<TValue, TError> Create()
    {
        var input = inputModelFactory.Create();
        var result = new SelectInputGroupModel<TValue, TError>(input);

        return result;
    }
}
