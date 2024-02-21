namespace RhoMicro.ApplicationFramework.Presentation.LocalGui.App;
internal class Program
{
    [STAThread]
    private static void Main(String[] args)
    {
        var builder = PhotinoBlazorAppBuilder.CreateDefault();

        // register root component and selector
        builder.RootComponents.Add<EntryPoint>("app");

        AddConfiguration(builder);
        AddLogging(builder);

        using var integrator = SimpleInjectorBlazorIntegrator.CreateDefault();
        //integrate into the app
        PhotinoBlazorApp app;
        try
        {
            app = AdaptIntegrate(builder, integrator);
        } catch(Exception ex)
        {
            PrintFastExit(ex.ToString());
            return;
        }

        ConfigureWindow(app);

        AppDomain.CurrentDomain.UnhandledException += (sender, error)
            => app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString(), icon: PhotinoNET.PhotinoDialogIcon.Error);

        app.Run();
    }

    private static void AddConfiguration(PhotinoBlazorAppBuilder builder)
    {
        _ = builder.Configuration.AddJsonFile("appsettings.json");
        var environmentAppsettings = $"appsettings.{builder.Environment.EnvironmentName}.json";
        if(File.Exists(environmentAppsettings))
            _ = builder.Configuration.AddJsonFile(environmentAppsettings);
    }

    private static void AddLogging(PhotinoBlazorAppBuilder builder) =>
        //TODO: https://stackoverflow.com/a/68363461
        throw new NotImplementedException();

    private static PhotinoBlazorApp AdaptIntegrate(PhotinoBlazorAppBuilder builder, SimpleInjectorBlazorIntegrator integrator)
    {
        var app = integrator.Integrate(
           builder,
           Root.LocalGui.Compose,
           typeof(Presentation.Views.Blazor._Imports).Assembly,
           typeof(LocalGui.EntryPoint).Assembly);

        return app;
    }
    private static void ConfigureWindow(PhotinoBlazorApp app)
    {
        _ = app.MainWindow
            .SetIconFile("favicon.ico")
            .SetTitle("Taskforce Builder")
            .SetMaximized(true)
#if !DEBUG
            .SetDevToolsEnabled(false)
            .SetContextMenuEnabled(false)
#endif
            ;
    }
    private static void PrintFastExit(String message)
    {
        //await integrator logs
        Thread.Sleep(2500);
        Console.WriteLine(String.Concat(Enumerable.Repeat('─', 100)));
        Console.WriteLine(message);
    }
}
