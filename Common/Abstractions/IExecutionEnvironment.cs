namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Provides information on the process execution environment.
/// </summary>
public interface IExecutionEnvironment
{
    /// <summary>
    /// Gets the configuration under which the process is running.
    /// </summary>
    IEnvironmentConfiguration Configuration { get; }
    /// <summary>
    /// Gets information on the operating system hosting the process.
    /// </summary>
    IOperatingSystemInfo OperatingSystem { get; }
    /// <summary>
    /// Gets the platform to which to process is deployed.
    /// </summary>
    DeploymentPlatform DeploymentPlatform { get; }
}
