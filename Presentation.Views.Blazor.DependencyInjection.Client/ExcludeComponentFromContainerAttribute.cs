namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using System;

/// <summary>
/// Marks the target component to be excluded from conventional DI registrations via the containing assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ExcludeComponentFromContainerAttribute:Attribute;