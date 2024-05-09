using RhoMicro.ApplicationFramework.Hosting;

using Composition;
using Presentation.WebGui.Client;

await WebClientGuiApp.CreateBuilder(out var builder, s => s.Args = args)
    .AddBlazor()
    .ConfigureOptions(o => o.Composer += Composers.WebGuiClient)
    .ConfigureCapabilities(c =>
    {
        _ = c.Components
            .Add(typeof(Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(EntryPoint).Assembly);
    })
    .Build()
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
