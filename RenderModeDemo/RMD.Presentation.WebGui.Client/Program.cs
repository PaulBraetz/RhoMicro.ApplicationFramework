using RhoMicro.ApplicationFramework.Hosting;

using RMD.Composition;
using RMD.Presentation.WebGui.Client;

await WebClientGuiApp.CreateBuilder(out var builder, s => s.Args = args)
    .AddBlazor()
    .ConfigureOptions(o => o.Composer += Composers.WebGuiClient)
    .ConfigureCapabilities(c =>
    {
        _ = c.Components
            .Add(typeof(RMD.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(EntryPoint).Assembly);
    })
    .Build()
    .RunAsync();
