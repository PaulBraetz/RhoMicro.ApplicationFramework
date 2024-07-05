namespace RhoMicro.ApplicationFramework.Aspects;

using System;

/// <summary>
/// Provides the generator with the required service and request interface type information.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
#if GENERATOR
[RhoMicro.CodeAnalysis.GenerateFactory]
[RhoMicro.CodeAnalysis.IncludeFile]
#endif
public sealed partial class ServiceSettingsAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the interface type to use for requests. This type must contain exactly one generic parameter.
    /// </summary>
    public Type? RequestInterfaceType { get; set; }
    /// <summary>
    /// Gets or sets the interface type to use for services. This type must contain exactly two generic parameter.
    /// </summary>
    public Type? ServiceInterfaceType { get; set; }
    /// <summary>
    /// Gets or sets the attribute type to use for annotating DI registration data.
    /// This type must provide the following properties:
    /// <list type="bullet">
    /// <item><see langword="public"/> <see cref="Type"/> <c>RequestType { <see langword="get"/>; <see langword="set"/>; }</c></item>
    /// <item><see langword="public"/> <see cref="Type"/> <c>ResultType { <see langword="get"/>; <see langword="set"/>; }</c></item>
    /// <item><see langword="public"/> <see cref="Type"/> <c>ServiceType { <see langword="get"/>; <see langword="set"/>; }</c></item>
    /// <item><see langword="public"/> <see cref="Type"/> <c>ImplementationType { <see langword="get"/>; <see langword="set"/>; }</c></item>
    /// <item><see langword="public"/> <see cref="Type"/> <c>AdapterType { <see langword="get"/>; <see langword="set"/>; }</c></item>
    /// </list>
    /// </summary>
    public Type? ServiceInjectionInfoAttributeType { get; set; }
    /// <summary>
    /// Gets or sets the default visibility of generated members.
    /// When set to <see cref="ServiceVisibility.Default"/>, generated types will be <see langword="internal"/>.
    /// </summary>
    public ServiceVisibility DefaultVisibility { get; set; } = ServiceVisibility.Default;
}