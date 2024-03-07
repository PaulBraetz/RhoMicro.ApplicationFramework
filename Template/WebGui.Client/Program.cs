using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Presentation.WebGui;

using Template.Composition;
using Template.Views;
using Template.WebGui.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var app = builder.IntegrateSimpleInjectorWeb(
    TemplateComposers.WebGuiClient,
    [typeof(EntryPoint).Assembly, typeof(App).Assembly],
    out var containerLifetime);

using var _ = containerLifetime;

await app.RunAsync();