namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;

/// <summary>
/// Default implementation of <see cref="IComponentRenderModeSettings"/>.
/// </summary>
/// <param name="ApplyRenderMode"></param>
public sealed record ComponentRenderModeSettings(Boolean ApplyRenderMode) : IComponentRenderModeSettings;
