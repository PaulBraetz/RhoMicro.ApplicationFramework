namespace RhoMicro.ApplicationFramework.Common.Environment;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Default implementation of <see cref="IDeploymentPlatformProvider"/>.
/// </summary>
/// <param name="DeploymentPlatform">
/// Gets the <see cref="Common.Environment.DeploymentPlatform"/> to which the current application has been deployed.
/// </param>
public sealed record DeploymentPlatformProvider(DeploymentPlatform DeploymentPlatform) : IDeploymentPlatformProvider;