namespace RMD.Composition;

using RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Contains template composers.
/// </summary>
public static class Composers
{
    private static IComposer Default { get; } = Composer.Empty;
    /// <summary>
    /// Gets the default composition root for web application servers.
    /// </summary>
    public static IComposer WebGui { get; } = Default;
    /// <summary>
    /// Gets the default composition root for web application clients.
    /// </summary>
    public static IComposer WebGuiClient { get; } = Default;
    /// <summary>
    /// Gets the default composition root for local application.
    /// </summary>
    public static IComposer LocalGui { get; } = Default;
}
