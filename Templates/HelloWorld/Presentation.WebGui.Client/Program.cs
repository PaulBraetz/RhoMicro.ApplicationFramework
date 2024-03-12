using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using RhoMicro.ApplicationFramework.Presentation.WebGui;

using HelloWorld.Presentation.Composition;
using HelloWorld.Presentation.Views;
using HelloWorld.Presentation.WebGui.Client;
using HelloWorld.Presentation.Views.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var app = builder.IntegrateSimpleInjectorWeb(
    Composers.WebGuiClient,
    [typeof(EntryPoint).Assembly, typeof(App).Assembly],
    out var containerLifetime);

using var _ = containerLifetime;

await app.RunAsync();
