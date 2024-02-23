namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Represents a composition root, ready to define its Object tree to a <see cref="Container"/> instance.
/// </summary>
public interface IComposer
{
    /// <summary>
    /// Defines the desired Object tree to a container.
    /// </summary>
    /// <param name="container">The container which to register definitions to.</param>
    void Compose(Container container);
}
