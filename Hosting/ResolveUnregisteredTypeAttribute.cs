namespace RhoMicro.ApplicationFramework.Hosting;
using System;

/// <summary>
/// Marks the target type to be resolvable without registration to the container.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ResolveUnregisteredTypeAttribute:Attribute;
