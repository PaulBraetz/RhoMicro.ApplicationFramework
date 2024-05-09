using RhoMicro.ApplicationFramework.Hosting;

using TA.Composition;
using TA.Presentation.WebGui.Client;

await WebClientGuiApp.CreateBuilder(out var builder, s => s.Args = args)
    .AddBlazor()
    .ConfigureOptions(o => o.Composer += Composers.WebGuiClient)
    .ConfigureCapabilities(c =>
    {
        _ = c.Components
            .Add(typeof(TA.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(EntryPoint).Assembly);
    })
    .Build()
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
