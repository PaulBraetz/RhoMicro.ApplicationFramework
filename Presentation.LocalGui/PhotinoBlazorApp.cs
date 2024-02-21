namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;

using PhotinoNET;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Implements the photino blazor application.
/// </summary>
public sealed class PhotinoBlazorApp : IApplication
{
    internal PhotinoBlazorApp(
        IServiceProvider services,
        BlazorWindowRootComponents rootComponents,
        PhotinoWindow mainWindow,
        PhotinoWebViewManager windowManager,
        PhotinoBlazorAppOptions options)
    {
        Services = services;
        RootComponents = rootComponents;
        MainWindow = mainWindow;
        WindowManager = windowManager;
        Options = options;
    }

    internal PhotinoBlazorAppOptions Options { get; }
    /// <inheritdoc/>
    public IServiceProvider Services { get; }
    /// <summary>
    /// Gets configuration for the root components in the window.
    /// </summary>
    internal BlazorWindowRootComponents RootComponents { get; }
    /// <summary>
    /// Gets the main window.
    /// </summary>
    public PhotinoWindow MainWindow { get; }
    /// <summary>
    /// Gets the web view manager.
    /// </summary>
    public PhotinoWebViewManager WindowManager { get; }
    /// <summary>
    /// Runs the application.
    /// </summary>
    public void Run()
    {
        WindowManager.Navigate(Options.IndexPath);
        MainWindow.WaitForClose();
    }
}
