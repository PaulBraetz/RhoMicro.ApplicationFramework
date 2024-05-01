using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Hosting;

using RMD.Composition;
using RMD.Presentation.WebGui.Client;

var app = WebClientGuiApp.CreateBuilder(out var builder, s =>
    {
        s.Args = args;
    })
    .ConfigureOptions(o =>
    {
        o.Composer = Composers.CreateWebGuiClient(builder.Capabilities);
    })
    .ConfigureCapabilities(c =>
    {
        _ = c.Components
            .Add(typeof(RMD.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(EntryPoint).Assembly);
    })
    .Build();

Console.WriteLine("Config:");
app.UnderlyingApp.Configuration.AsEnumerable().ForEach(kvp => Console.WriteLine(new { Path = kvp.Key, kvp.Value }));

await app
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
