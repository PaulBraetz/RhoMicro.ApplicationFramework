namespace RhoMicro.ApplicationFramework.Aspects;

using System;

/// <summary>
/// Annotates a service method for AOP service generation.
/// No request type or service interface will be generated.
/// Use this attribute when defining the service interface and request type manually or externally.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
#if GENERATOR
[RhoMicro.CodeAnalysis.GenerateFactory]
[RhoMicro.CodeAnalysis.IncludeFile]
#endif
internal sealed partial class ServiceMethodImplementationAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the external service interface implemented by this service.
    /// </summary>
    public
#if !GENERATOR
        required 
#endif
        Type Service
    { get; set; }
#if GENERATOR
    = null!;
#endif
    /// <summary>
    /// Gets or sets the request type to generate the service implementation for.
    /// </summary>
    public
#if !GENERATOR
        required 
#endif
        Type Request
    { get; set; }
#if GENERATOR
    = null!;
#endif
}
