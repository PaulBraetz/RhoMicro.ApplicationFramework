namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Base class for input controls that provide a label, description and error.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by the model.</typeparam>
/// <typeparam name="TError">The type of error displayed by the model.</typeparam>
public class InputGroup<TValue, TError> : InputGroupSpecialized<IInputModel<TValue, TError>, TValue, TError>;
