namespace RMD.Presentation.LocalGui;

using System;

using NReco.Logging.File;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;
using RhoMicro.ApplicationFramework.Hosting;

using RMD.Composition;
using RMD.Presentation.Views.Blazor;

class Program
{
    [STAThread]
    static void Main(String[] args) =>
        LocalGuiApp.CreateBuilder(out var builder, s => s.Args = args)
        .AddBlazor()
        .ConfigureBuilder(b => b.RootComponents.Add<EntryPoint>("app"))
        .ConfigureOptions(o => o.Composer += Composers.LocalGui)
        .ConfigureCapabilities(c =>
        {
            var loggingSection = c.Configuration.Build().GetSection("Logging");
            _ = c.Logging.AddFile(loggingSection);
            _ = c.Components
                .Add(typeof(App).Assembly)
                .Add(typeof(EntryPoint));
        })
        .Build()
        .ConfigureUnderlyingApp((app, container) =>
        {
            var isDevToolsEnabled = container.GetInstance<IEnvironmentConfiguration>()
                .IsDevelopment();

            _ = app.MainWindow
                .SetIconFile("favicon.ico")
                .SetTitle("Render Mode Demo Local")
                .SetDevToolsEnabled(isDevToolsEnabled);

            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
                app.MainWindow.ShowMessage("Exception", args.Exception.ToString());
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                app.MainWindow.ShowMessage("Fatal exception", args.ExceptionObject.ToString());
        })
        .Run();
}
