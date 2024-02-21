namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Factory for producing instances of <see cref="SelectInputGroupModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public sealed class SelectInputGroupModelFactory<TValue, TError> : IFactory<ISelectInputGroupModel<TValue, TError>>
{
    private readonly IFactory<ISelectInputModel<TValue, TError>> _inputModelFactory;
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="inputModelFactory">The factory to use when producing the input model for results.</param>
    public SelectInputGroupModelFactory(IFactory<ISelectInputModel<TValue, TError>> inputModelFactory)
    {
        _inputModelFactory = inputModelFactory;
    }

    /// <inheritdoc/>
    public ISelectInputGroupModel<TValue, TError> Create()
    {
        var input = _inputModelFactory.Create();
        var result = new SelectInputGroupModel<TValue, TError>(input);

        return result;
    }
}
