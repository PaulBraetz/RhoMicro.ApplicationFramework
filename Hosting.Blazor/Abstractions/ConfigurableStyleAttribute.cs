namespace RhoMicro.ApplicationFramework.Hosting;

/// <summary>
/// Base class for marking configurable stylings.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public abstract class ConfigurableStyleAttribute : Attribute
{
    internal abstract Type StyleType { get; }
    internal abstract Type StyleSettingsType { get; }
}
