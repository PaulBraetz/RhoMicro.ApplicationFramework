namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;

using Microsoft.Extensions.DependencyInjection;

using PhotinoNET;

internal sealed class PhotinoBlazorAppFactory
{
#pragma warning disable CA1822 // Mark members as static
    public PhotinoBlazorApp Create(IServiceProvider services, RootComponentList rootComponents)
#pragma warning restore CA1822 // Mark members as static
    {
        var blazorWindowRootComponents = services.GetRequiredService<BlazorWindowRootComponents>();
        var windowManager = services.GetRequiredService<PhotinoWebViewManager>();
        var mainWindow = services.GetRequiredService<PhotinoWindow>()
            .Apply(services.GetService<DefaultPhotinoMainWindowOptions>() ?? new())
            .RegisterCustomSchemeHandler(PhotinoWebViewManager.BlazorAppScheme, windowManager.HandleWebRequest);
        var options = services.GetService<PhotinoBlazorAppOptions>() ?? new();

        foreach(var component in rootComponents.Enumerate())
        {
            blazorWindowRootComponents.Add(component.Item1, component.Item2);
        }

        var result = new PhotinoBlazorApp(services, blazorWindowRootComponents, mainWindow, windowManager, options);

        return result;
    }
}
