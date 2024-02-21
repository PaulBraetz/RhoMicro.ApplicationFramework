namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Integration helper for integrating SimpleInjector into blazor component activation.
/// View properties that are to be injected via must be annotated using this attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectedAttribute : Attribute
{
}
