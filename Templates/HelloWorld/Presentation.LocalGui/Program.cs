namespace HelloWorld.Presentation.LocalGui;

using System;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;
using RhoMicro.ApplicationFramework.Hosting;

using HelloWorld.Composition;
using HelloWorld.Presentation.Views.Blazor;

class Program
{
    [STAThread]
    static void Main(String[] args) =>
        LocalGuiApp.CreateBuilder(out var builder, s =>
        {
            s.Args = args;
#if DEBUG
            s.EnvironmentConfiguration = EnvironmentConfiguration.Development;
#else
            s.EnvironmentConfiguration = EnvironmentConfiguration.Production;
#endif
        })
        .AddAppSettings()
        .ConfigureBuilder(b => b.RootComponents.Add<EntryPoint>("app"))
        .ConfigureOptions(o => o.Composer = Composers.CreateLocalGui(builder.Capabilities))
        .ConfigureCapabilities(c => c.Components
            .Add(typeof(App).Assembly)
            .Add(typeof(EntryPoint)))
        .Build()
        .ConfigureUnderlyingApp((app, container) =>
        {
            var isDevToolsEnabled = container.GetInstance<IEnvironmentConfiguration>()
                .IsDevelopment();

            _ = app.MainWindow
                .SetIconFile("favicon.ico")
                .SetTitle("Photino Blazor Sample")
                .SetDevToolsEnabled(isDevToolsEnabled);

            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
                app.MainWindow.ShowMessage("Exception", args.Exception.ToString());
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                app.MainWindow.ShowMessage("Fatal exception", args.ExceptionObject.ToString());
        })
        .Run();
}
