namespace RhoMicro.ApplicationFramework.Common;

using System;

/// <summary>
/// Provides the types required to properly register an AOP service.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
#if !GENERATOR
public
#endif
sealed class ServiceInjectionInfoAttribute : Attribute
{
    /// <summary>
    /// Gets the type of request handled by the service.
    /// </summary>
    public required Type RequestType { get; init; }
    /// <summary>
    /// Gets the type of result produced by the service.
    /// </summary>
    public required Type ResultType { get; init; }
    /// <summary>
    /// Gets the type of traditional service to register.
    /// </summary>
    public required Type ServiceType { get; init; }
    /// <summary>
    /// Gets the type of service implementing <c>IService&lt;RequestType, ResultType&gt;</c>
    /// </summary>
    public required Type ImplementationType { get; init; }
    /// <summary>
    /// Gets the type adapting <c>ImplementationType</c> onto <c>ServiceType</c>.
    /// </summary>
    public required Type AdapterType { get; init; }
}
