namespace RhoMicro.ApplicationFramework.Template_Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Contains template composers.
/// </summary>
public static class TemplateComposers
{
    private static IComposer Default { get; } = Composer.Create(
        Presentation.Models,
        BlazorViewComposers.Default,
#if DEBUG
        Common.Debug
#else
        Common.Release
#endif
        );
    /// <summary>
    /// Gets the default composition root for web application servers.
    /// </summary>
    public static IComposer WebGui { get; } = Composer.Create(Default);
    /// <summary>
    /// Gets the default composition root for web application clients.
    /// </summary>
    public static IComposer WebGuiClient { get; } = Composer.Create(Default);
    /// <summary>
    /// Gets the default composition root for local application.
    /// </summary>
    public static IComposer LocalGui { get; } = Composer.Create(Default);
}
