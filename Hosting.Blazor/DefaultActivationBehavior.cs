namespace RhoMicro.ApplicationFramework.Hosting;

using System.Reflection;

/// <summary>
/// Implementation of <see cref="IUnregisteredComponentActivationBehavior"/> that defines 
/// the following components as activatable without registration:
/// <list type="bullet">
/// <item>all components found in the <c>Microsoft.AspNetCore.Components</c> namespace</item>
/// </list>
/// </summary>
public sealed class DefaultActivationBehavior : IUnregisteredComponentActivationBehavior
{
    /// <inheritdoc/>
    public Boolean Activate(Type unregisteredComponentType)
    {
        ArgumentNullException.ThrowIfNull(unregisteredComponentType);

        if(unregisteredComponentType.Namespace?.StartsWith("Microsoft.AspNetCore.Components", StringComparison.Ordinal) ?? false)
            return true;

        return false;
    }
}
