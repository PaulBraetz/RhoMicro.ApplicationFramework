namespace RhoMicro.ApplicationFramework.Presentation.WebGui;

using System;

using Microsoft.AspNetCore.Builder;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

internal sealed class WebApplicationAdapter(WebApplication app) : IApplication
{
    public WebApplication App => app;
    public IServiceProvider Services => app.Services;
}
