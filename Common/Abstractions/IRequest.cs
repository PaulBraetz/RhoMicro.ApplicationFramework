namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a request parameter object whose execution must result in a specific result type.
/// </summary>
/// <typeparam name="TResult">The type of result to be produced by a service.</typeparam>
#if !GENERATOR
public
#endif
    interface IRequest<TResult>;
