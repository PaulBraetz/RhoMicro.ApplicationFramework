namespace RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Environment;

/// <summary>
/// Provides the <see cref="Common.Environment.DeploymentPlatform"/> to which the current application has been deployed.
/// </summary>
public interface IDeploymentPlatformProvider
{
    /// <summary>
    /// Gets the <see cref="Common.Environment.DeploymentPlatform"/> to which the current application has been deployed.
    /// </summary>
    DeploymentPlatform DeploymentPlatform { get; }
}