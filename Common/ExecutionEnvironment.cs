namespace RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Default implementation of <see cref="IExecutionEnvironment"/>.
/// </summary>
/// <param name="Configuration"></param>
/// <param name="OperatingSystem"></param>
/// <param name="DeploymentPlatform"></param>
public sealed record ExecutionEnvironment(IEnvironmentConfiguration Configuration, IOperatingSystemInfo OperatingSystem, DeploymentPlatform DeploymentPlatform) : IExecutionEnvironment;
