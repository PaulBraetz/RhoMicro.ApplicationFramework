namespace RhoMicro.ApplicationFramework.Composition.Presentation.Views.Blazor;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Contains composers able to compose blazor view object graphs.
/// </summary>
public static class BlazorViewComposers
{
    /// <summary>
    /// Gets the default composer instance.
    /// </summary>
    public static IComposer Default { get; } = Composer.Create(c => c.RegisterSingleton<ICssStyle, CssStyle>());
}
