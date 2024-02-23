namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="ISelectInputGroupModel{TValue, TError}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value to select.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="input">The input to use.</param>
public sealed class SelectInputGroupModel<TValue, TError>(ISelectInputModel<TValue, TError> input)
    : InputGroupModel<ISelectInputModel<TValue, TError>, IOptionModel<TValue>?, TError>(input), ISelectInputGroupModel<TValue, TError>;
