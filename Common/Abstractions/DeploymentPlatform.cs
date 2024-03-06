namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Provides different deployment target platforms.
/// </summary>
public enum DeploymentPlatform
{
    /// <summary>
    /// Represents an unknown platform.
    /// </summary>
    Unknown,
    /// <summary>
    /// Represents a server application platform.
    /// </summary>
    Server,
    /// <summary>
    /// Represents a local desktop application platform
    /// </summary>
    Desktop
}