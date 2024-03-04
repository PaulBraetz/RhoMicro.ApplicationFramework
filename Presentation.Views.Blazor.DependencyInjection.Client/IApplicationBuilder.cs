namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Generic abstraction for an application. 
/// Example implementations are wrappers for the 
/// WebApplicationBuilder or PhotinoApplicationBuilder
/// </summary>
/// <typeparam name="TApplication">The type of application built.</typeparam>
public interface IApplicationBuilder<TApplication>
    where TApplication : IApplication
{
    /// <summary>
    /// Gets the builders service collection.
    /// </summary>
    IServiceCollection Services { get; }
    /// <summary>
    /// Builds an application.
    /// </summary>
    /// <returns></returns>
    TApplication Build();
}
