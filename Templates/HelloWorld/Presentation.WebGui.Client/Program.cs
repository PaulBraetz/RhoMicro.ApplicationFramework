using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Hosting;

using HelloWorld.Composition;
using HelloWorld.Presentation.WebGui.Client;

await WebClientGuiApp.CreateBuilder(out var builder, s => s.Args = args)
    .ConfigureOptions(o => o.Composer = Composers.CreateWebGuiClient(builder.Capabilities))
    .ConfigureCapabilities(c =>
    {
        _ = c.Components
            .Add(typeof(HelloWorld.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(EntryPoint).Assembly);
    })
    .Build()
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
