using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Presentation.WebGui;
using RhoMicro.ApplicationFramework.Template_Composition;

using Template_Views;

using Template_WebGui.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var app = builder.IntegrateSimpleInjectorWeb(
    TemplateComposers.WebGuiClient,
    [typeof(EntryPoint).Assembly, typeof(App).Assembly],
    out var containerLifetime);

using var _ = containerLifetime;

await app.RunAsync();
