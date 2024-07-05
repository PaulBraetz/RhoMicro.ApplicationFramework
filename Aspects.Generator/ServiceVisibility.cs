namespace RhoMicro.ApplicationFramework.Aspects;

/// <summary>
/// Provides possible service visibilities for generated service interface and request types.
/// </summary>
#if GENERATOR
[RhoMicro.CodeAnalysis.IncludeFile]
#endif
public enum ServiceVisibility
{
    /// <summary>
    /// The generated types will have the visibility set by <see cref="ServiceSettingsAttribute"/>.
    /// </summary>
    Default,
    /// <summary>
    /// The generated types will be <see langword="public"/>.
    /// </summary>
    Public,
    /// <summary>
    /// The generated members will be <see langword="internal"/>.
    /// </summary>
    Internal
}