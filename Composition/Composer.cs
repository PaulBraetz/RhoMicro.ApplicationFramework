namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Contains  definitions for different application object graph roots.
/// </summary>
public static partial class Composer
{
    /// <summary>
    /// Gets an empty composer instance. This composer will not compose an object graph.
    /// </summary>
    public static IComposer Empty { get; } = new EmptyComposer();

    /// <summary>
    /// Creates a new composition root using the composition strategy provided.
    /// </summary>
    /// <param name="compose">The strategy using which to compose the application.</param>
    /// <returns>A new composition root based on the composition strategy passed.</returns>
    public static IComposer Create(Action<Container> compose)
    {
        ArgumentNullException.ThrowIfNull(compose);

        return new Strategy(compose);
    }

    /// <summary>
    /// Creates a new composite composition root using the roots provided.
    /// </summary>
    /// <param name="roots">The roots defining the composite composition.</param>
    /// <returns>A new composite composition root based on the roots provided.</returns>
    public static IComposer Create(params IComposer[] roots) =>
        new Strategy(c =>
        {
            for(var i = 0; i < roots.Length; i++)
                roots[i].Compose(c);
        });
}
