using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Presentation.WebGui;

using RenderModeDemo.Presentation.Composition;
using RenderModeDemo.Presentation.Views;
using RenderModeDemo.Presentation.WebGui.Client;
using RenderModeDemo.Presentation.Views.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var app = builder.IntegrateSimpleInjectorWeb(
    TemplateComposers.WebGuiClient,
    [typeof(EntryPoint).Assembly, typeof(App).Assembly],
    out var containerLifetime);

using var _ = containerLifetime;

await app.RunAsync();
