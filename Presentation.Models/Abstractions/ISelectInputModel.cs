namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents an input model for selecting one of several options, e.g.: radio buttons, checkbox list etc..
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public interface ISelectInputModel<TValue, TError> : IInputModel<IOptionModel<TValue>?, TError>
{
    /// <summary>
    /// Gets the values for this select model.
    /// </summary>
    IEnumerable<IOptionModel<TValue>> GetOptions();
}
