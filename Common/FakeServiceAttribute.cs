namespace RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Helper attribute for annotating fake service implementations. This attribute is inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class FakeServiceAttribute : Attribute;