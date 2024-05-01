namespace RhoMicro.ApplicationFramework.Hosting;

/// <summary>
/// Represents blazor app builder capabilities.
/// </summary>
public class BlazorAppBuilderCapabilities : AppBuilderCapabilities
{
    /// <summary>
    /// Gets the set of components to be made available to the app.
    /// </summary>
    public required ComponentTypeSet Components { get; init; }
}
