namespace RMD.Composition;

using RhoMicro.ApplicationFramework.Common.Environment;
using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Hosting;

/// <summary>
/// Contains template composers.
/// </summary>
public static class Composers
{
    private static IComposer CreateDefault(AppBuilderCapabilities capabilities) => Composer.Create(
        PresentationComposers.HostingInformation,
        AspectComposers.Default,
        BlazorViewComposers.CreateStylesComposer(capabilities.Configuration.Build()));
    /// <summary>
    /// Gets the default composition root for web application servers.
    /// </summary>
    public static IComposer CreateWebGui(AppBuilderCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);

        return CreateDefault(capabilities);
    }
    /// <summary>
    /// Gets the default composition root for web application clients.
    /// </summary>
    public static IComposer CreateWebGuiClient(AppBuilderCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);

        return CreateDefault(capabilities);
    }
    /// <summary>
    /// Gets the default composition root for local application.
    /// </summary>
    public static IComposer CreateLocalGui(AppBuilderCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);

        return CreateDefault(capabilities);
    }
}
