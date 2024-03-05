﻿namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Non-specific abstraction of an application.
/// </summary>
public interface IApplication
{
    /// <summary>
    /// Gets the applications service provider.
    /// </summary>
    IServiceProvider Services { get; }
}