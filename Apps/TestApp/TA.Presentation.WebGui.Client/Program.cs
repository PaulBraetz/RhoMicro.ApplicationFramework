using RhoMicro.ApplicationFramework.Hosting;

using TA.Composition;
using TA.Presentation.WebGui.Client;

await WebClientGuiApp.CreateBuilder(out var builder, s => s.Args = args)
    .AddBlazor()
    .AddApiServiceClients()
    .ConfigureOptions(o => o.Composer += Composers.WebGuiClient)
    .ConfigureCapabilities(c =>
    {
        _ = c.Components
            .Add(typeof(TA.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(EntryPoint).Assembly);
    })
    .AddTimeout()
    .Build()
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
