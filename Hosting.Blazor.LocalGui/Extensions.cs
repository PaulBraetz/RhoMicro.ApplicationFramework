namespace RhoMicro.ApplicationFramework.Hosting;

using Photino.Blazor;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Hosting</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds blazor to the local app builder.
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <returns>A new combined composer.</returns>
    public static LocalGuiAppBuilder AddBlazor(this LocalGuiAppBuilder appBuilder) =>
        appBuilder.AddBlazor<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();

    ///// <summary>
    ///// Adds default component models to the app builders capabilities.
    ///// </summary>
    //private static LocalGuiAppBuilder AddDefaultModels(this LocalGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultModels<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds default component views to the app builders capabilities.
    ///// </summary>
    //private static LocalGuiAppBuilder AddDefaultViews(this LocalGuiAppBuilder appBuilder) =>
    //    appBuilder.AddDefaultViews<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds appsettings to the app builder.
    ///// </summary>
    //private static LocalGuiAppBuilder AddAppSettings(this LocalGuiAppBuilder appBuilder) =>
    //    appBuilder.AddAppSettings<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();
    ///// <summary>
    ///// Adds configuration based file logging to the builders capabilities.
    ///// </summary>
    //private static LocalGuiAppBuilder AddFileLogging(this LocalGuiAppBuilder appBuilder, Action<FileLoggerOptions>? configureOptions = null) =>
    //    appBuilder.AddFileLogging<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(configureOptions);
    ///// <summary>
    ///// Adds configuration based console logging to the builders capabilities.
    ///// </summary>
    //private static LocalGuiAppBuilder AddConsoleLogging(this LocalGuiAppBuilder appBuilder, Action<ConsoleLoggerOptions>? configureOptions = null) =>
    //    appBuilder.AddConsoleLogging<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>(configureOptions);
    ///// <summary>
    ///// Adds appsettings and configuration to the app builder.
    ///// </summary>
    //private static LocalGuiAppBuilder AddConfiguration(this LocalGuiAppBuilder appBuilder)
    //{
    //    ArgumentNullException.ThrowIfNull(appBuilder);

    //    var configBuilder = new ConfigurationBuilder();

    //    var result = appBuilder
    //        .ConfigureBuilder(b => b.Services
    //            .AddSingleton<IConfigurationBuilder>(configBuilder)
    //            .AddSingleton(p => p.GetRequiredService<IConfigurationBuilder>().Build())
    //            .AddSingleton<IConfiguration>(p => p.GetRequiredService<IConfigurationRoot>())
    //        ).AddAppSettings<LocalGuiAppBuilder, LocalGuiApp, PhotinoBlazorAppBuilder, PhotinoBlazorApp, BlazorAppBuilderCapabilities>();

    //    return result;
    //}
}
