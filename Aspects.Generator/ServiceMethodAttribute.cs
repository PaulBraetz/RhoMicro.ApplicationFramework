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
internal sealed partial class ServiceMethodAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the kind of request type to generate for the targeted
    /// service method.
    /// </summary>
    public RequestTypeKind RequestTypeKind { get; set; }
    /// <summary>
    /// Gets or sets the namespace of the request and service interface type.
    /// </summary>
    public String? Namespace { get; set; } = null;
    /// <summary>
    /// Gets or sets the service interface. If set to <see langword="null"/>, a
    /// name based on the containing class name will be used.
    /// </summary>
    public String? ServiceInterfaceName { get; set; } = null;
    /// <summary>
    /// Gets or sets the visibility to generate the request and service
    /// interface types with. If set to <see cref="ServiceVisibility.Default"/>,
    /// a default visibility from <see cref="ServiceSettingsAttribute"/> will be
    /// used.
    /// </summary>
    public ServiceVisibility Visibility { get; set; } = ServiceVisibility.Default;
}
