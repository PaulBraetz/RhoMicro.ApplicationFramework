namespace RhoMicro.ApplicationFramework.Hosting;

/// <summary>
/// Implementation of <see cref="IUnregisteredComponentActivationBehavior"/> that defines all components inside the <c>Microsoft.AspNetCore.Components</c> namespace as activatable without registration.
/// </summary>
public sealed class AspNetCoreActivationBehavior : IUnregisteredComponentActivationBehavior
{
    /// <inheritdoc/>
    public Boolean Activate(Type unregisteredComponentType) =>
        ( unregisteredComponentType ?? throw new ArgumentNullException(nameof(unregisteredComponentType)) )
        .Namespace?.StartsWith("Microsoft.AspNetCore.Components", StringComparison.OrdinalIgnoreCase)
        ?? false;
}
