namespace RhoMicro.ApplicationFramework.Presentation.WebGui;

using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

internal sealed class WebAssemblyHostAdapter(WebAssemblyHost app) : IApplication
{
    public WebAssemblyHost App => app;
    public IServiceProvider Services => app.Services;
}
