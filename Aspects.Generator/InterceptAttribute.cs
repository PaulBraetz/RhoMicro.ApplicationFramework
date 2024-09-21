namespace RhoMicro.ApplicationFramework.Aspects;

using System;

/// <summary>
/// Annotates parameters for interception by an 
/// <see cref="RhoMicro.ApplicationFramework.Common.Abstractions.IInterceptor{T}"/>
/// before being passed to the service method.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
#if GENERATOR
[RhoMicro.CodeAnalysis.GenerateFactory]
[RhoMicro.CodeAnalysis.IncludeFile]
#endif
internal sealed partial class InterceptAttribute : Attribute { }
