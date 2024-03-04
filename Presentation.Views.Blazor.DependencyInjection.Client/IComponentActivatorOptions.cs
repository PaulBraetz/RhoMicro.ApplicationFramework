namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

/// <summary>
/// Provides options for creating components.
/// </summary>
public sealed class RenderModeOptions
{
    /// <summary>
    /// Gets a value indicating whether to omit render mode wrapping behavior when instantiating component instances
    /// annotated with <see cref="OptionalRenderModeAttribute"/>.
    /// </summary>
    public Boolean OmitRenderModes { get; init; }
}
