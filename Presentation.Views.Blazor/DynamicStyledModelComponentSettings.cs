namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Provides settings for rendering instances of <see cref="DynamicModelComponent{TModel}"/>.
/// </summary>
/// <param name="ComponentType">The type of component to render in place of the dynamic component.</param>
public sealed record DynamicModelComponentSettings<TModel>(ModelComponentType ComponentType) : DynamicStyledModelComponentSettings<TModel, ICssStyle>(ComponentType);

/// <summary>
/// Provides settings for rendering instances of <see cref="DynamicStyledModelComponent{TModel, TStyle}"/>.
/// </summary>
/// <param name="ComponentType">The type of component to render in place of the dynamic component.</param>
public record DynamicStyledModelComponentSettings<TModel, TStyle>(ModelComponentType ComponentType);
