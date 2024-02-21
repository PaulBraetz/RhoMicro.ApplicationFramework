namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="ISelectInputGroupModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public sealed class SelectInputGroupModel<TValue, TError> :
    InputGroupModel<ISelectInputModel<TValue, TError>, IOptionModel<TValue>?, TError>,
    ISelectInputGroupModel<TValue, TError>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="input">The input to use.</param>
    public SelectInputGroupModel(
        ISelectInputModel<TValue, TError> input)
        : base(input)
    {

    }
}
