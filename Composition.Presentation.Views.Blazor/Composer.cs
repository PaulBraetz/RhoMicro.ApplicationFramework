namespace RhoMicro.ApplicationFramework.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

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
    public static IComposer Default { get; } = Composer.Create(c =>
    {
        c.RegisterSingleton<ICssStyle, CssStyle>();
    });
}
