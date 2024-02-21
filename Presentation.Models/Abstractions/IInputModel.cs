namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a simple input field that contains information of the validity of the value entered.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public interface IInputModel<TValue, TError> : INotifyPropertyValueChanged
{
    /// <summary>
    /// Gets a value indicating the validity of <see cref="Value"/>.
    /// If <see cref="Validity"/> is set to <see cref="InputValidityType.None"/> or <see cref="InputValidityType.Valid"/>, 
    /// any value retrieved from <see cref="ErrorValue"/> should be ignored.
    /// </summary>
    InputValidityType Validity { get; }
    /// <summary>
    /// Sets <see cref="Validity"/> to <see cref="InputValidityType.Invalid"/> and <see cref="ErrorValue"/> to <paramref name="errorValue"/>.
    /// </summary>
    /// <param name="errorValue">The error to set <see cref="ErrorValue"/> to.</param>
    void SetInvalid(TError errorValue);
    /// <summary>
    /// Sets <see cref="Validity"/> to <see cref="InputValidityType.Valid"/> and <see cref="ErrorValue"/> to its initial value.
    /// </summary>
    void SetValid();
    /// <summary>
    /// Sets <see cref="Validity"/> to <see cref="InputValidityType.None"/> and <see cref="ErrorValue"/> to its initial value.
    /// </summary>
    void UnsetValidity();
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    TValue Value { get; set; }
    /// <summary>
    /// Gets or sets the autocomplete value to be recommended to the user.
    /// </summary>
    TValue AutoCompleteValue { get; }
    /// <summary>
    /// Gets or sets a value indicating whether the value returned by 
    /// <see cref="AutoCompleteValue"/> should be reccommended to the user.
    /// </summary>
    Boolean AutoCompleteEnabled { get; set; }
    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    TError ErrorValue { get; }
    /// <summary>
    /// Gets or sets the placeholde to display before a value has been entered.
    /// </summary>
    String Placeholder { get; set; }
    /// <summary>
    /// Confirms the value set by pressing the enter key.
    /// </summary>
    Task Enter();
    /// <summary>
    /// Invoked after the input has been confirmed by calling <see cref="Enter"/>.
    /// </summary>
    event AsyncEventHandler<IAsyncEventArguments<TValue>>? Entered;
}
