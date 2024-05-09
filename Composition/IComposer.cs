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
    /// <summary>
    /// Concatenates two composer instances into a combined composer.
    /// </summary>
    /// <param name="a">The first composer to concatenate.</param>
    /// <param name="b">The second composer to concatenate.</param>
    /// <returns>A concatenation of the two composers passed.</returns>
    public static IComposer operator +(IComposer a, IComposer b) => Composer.Create(a, b);
}
