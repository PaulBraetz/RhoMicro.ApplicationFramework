namespace RhoMicro.ApplicationFramework.Aspects;

using System;

/// <summary>
/// Annotates a service method for AOP service generation.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
#if GENERATOR
[RhoMicro.CodeAnalysis.GenerateFactory]
[RhoMicro.CodeAnalysis.IncludeFile]
#endif
public sealed partial class ServiceMethodAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the kind of request type to generate for the targeted service method.
    /// </summary>
    public RequestTypeKind RequestTypeKind { get; set; }
    /// <summary>
    /// Gets or sets the namespace to generate the request and service interface type into.
    /// </summary>
    public String Namespace { get; set; } = String.Empty;
    /// <summary>
    /// Gets or sets the visibility to generate the request and service interface types with.
    /// </summary>
    public ServiceVisibility Visibility { get; set; } = ServiceVisibility.Public;
}
