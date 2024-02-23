namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents an input model for selecting one of several options, e.g.: radio buttons, checkbox list etc..
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public interface ISelectInputGroupModel<TValue, TError> :
    IInputGroupModel<ISelectInputModel<TValue, TError>, IOptionModel<TValue>?, TError>;