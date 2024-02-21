namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Contains  definitions for different application object graph roots.
/// </summary>
public static partial class Root
{
    /// <summary>
    /// Creates a new composition root using the composition strategy provided.
    /// </summary>
    /// <param name="compose">The strategy using which to compose the application.</param>
    /// <returns>A new composition root based on the composition strategy passed.</returns>
    public static IRoot Create(Action<Container> compose)
    {
        ArgumentNullException.ThrowIfNull(compose);

        return new Strategy(compose);
    }

    /// <summary>
    /// Creates a new composite composition root using the roots provided.
    /// </summary>
    /// <param name="roots">The roots defining the composite composition.</param>
    /// <returns>A new composite composition root based on the roots provided.</returns>
    public static IRoot Create(params IRoot[] roots) =>
        new Strategy(c =>
        {
            for(var i = 0; i < roots.Length; i++)
                roots[i].Compose(c);
        });
}
