namespace RhoMicro.ApplicationFramework.Composition;
/// <summary>
/// Marks target properties for property injection by simpleinjector.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectedAttribute : Attribute;