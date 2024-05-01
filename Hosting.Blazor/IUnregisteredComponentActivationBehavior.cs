namespace RhoMicro.ApplicationFramework.Hosting;

/// <summary>
/// Defines behavior when encountering unregistered components.
/// </summary>
public interface IUnregisteredComponentActivationBehavior
{
    /// <summary>
    /// Gets a value indicating whether to activate a component even though it has not been previously registered to the di container.
    /// </summary>
    /// <param name="unregisteredComponentType">The type of the unregistered component.</param>
    /// <returns>
    /// <see langword="true"/> if an instance of <paramref name="unregisteredComponentType"/> ahould be created; otherwise, <see langword="false"/>.
    /// </returns>
    Boolean Activate(Type unregisteredComponentType);
}