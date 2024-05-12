using RhoMicro.ApplicationFramework.Hosting;

using TailwindHelloWorld.Composition;
using TailwindHelloWorld.Presentation.WebGui.Client;

await WebClientGuiApp.CreateBuilder(out var builder, s => s.Args = args)
    .AddBlazor()
    .ConfigureOptions(o => o.Composer += Composers.WebGuiClient)
    .ConfigureCapabilities(c =>
    {
        _ = c.Components
            .Add(typeof(TailwindHelloWorld.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(EntryPoint));
    })
    .Build()
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
