namespace Template.LocalGui;

using System;

using Photino.Blazor;

using RhoMicro.ApplicationFramework.Presentation.LocalGui;

using Template.Composition;
using Template.Views;

class Program
{
    [STAThread]
    static void Main(String[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args).WithAdapter();

        // register root component and selector
        appBuilder.RootComponents.Add<EntryPoint>("app");

        var app = appBuilder.IntegrateSimpleInjectorLocal(
            TemplateComposers.LocalGui,
            [typeof(App).Assembly],
            out var containerLifetime);

        using var _0 = containerLifetime;

        // customize window
        _ = app.MainWindow.SetIconFile("favicon.ico").SetTitle("Photino Blazor Sample");

        AppDomain.CurrentDomain.FirstChanceException += (sender, args) => app.MainWindow.ShowMessage("Exception", args.Exception.ToString());
        AppDomain.CurrentDomain.UnhandledException += (sender, args) => app.MainWindow.ShowMessage("Fatal exception", args.ExceptionObject.ToString());

        app.Run();
    }
}
