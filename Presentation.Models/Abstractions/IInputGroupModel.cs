namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Generic input group model for obtaining input and displaying errors, a description and label.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public interface IInputGroupModel<TValue, TError> :
    IInputGroupModel<IInputModel<TValue, TError>, TValue, TError>
{ }
/// <summary>
/// Generic input group model for obtaining input and displaying errors, a description and label.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
/// <typeparam name="TInput">The type of input model used to obtain input.</typeparam>
public interface IInputGroupModel<TInput, TValue, TError> : INotifyPropertyValueChanged
    where TInput : IInputModel<TValue, TError>
{
    /// <summary>
    /// Gets the model representing the value and its validity state.
    /// </summary>
    TInput Input { get; }
    /// <summary>
    /// Gets or sets the prominent input label.
    /// </summary>
    String Label { get; set; }
    /// <summary>
    /// Gets or sets a less prominent input description.
    /// </summary>
    String Description { get; set; }
}