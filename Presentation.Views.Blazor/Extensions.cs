namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using System;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

internal static class Extensions
{
    public static String GetCssClass(this ICssStyle style) => String.Join(' ', style.Classes);
}
