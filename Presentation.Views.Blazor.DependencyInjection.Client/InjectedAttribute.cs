namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

/// <summary>
/// Integration helper for integrating SimpleInjector into blazor component activation.
/// View properties that are to be injected during the component instantiation 
/// must be annotated using this attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectedAttribute : Attribute;