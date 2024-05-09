namespace RhoMicro.ApplicationFramework.Hosting;
/// <summary>
/// Options for the <see cref="SimpleInjectorComponentActivator"/>.
/// </summary>
/// <param name="UnregisteredComponentActivationBehavior">
/// Behavior when encountering unregistered components.
/// </param>
public sealed record ComponentActivatorOptions(IUnregisteredComponentActivationBehavior UnregisteredComponentActivationBehavior)
{
    /// <summary>
    /// Initializes a new instance with default options.
    /// </summary>
    public ComponentActivatorOptions() : this(new DefaultActivationBehavior()) { }
}
