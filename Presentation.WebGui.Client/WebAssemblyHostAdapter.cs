namespace RhoMicro.ApplicationFramework.Presentation.WebGui.Client;

using System;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

internal sealed class WebAssemblyHostAdapter(WebAssemblyHost app) : IApplication
{
    public WebAssemblyHost App => app;
    public IServiceProvider Services => app.Services;
}
