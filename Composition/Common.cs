namespace RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Contains object graph definitions for common object graphs.
/// </summary>
public static class Common
{
    /// <summary>
    /// Gets a composer able to compose common debug object graphs.
    /// </summary>
    public static IComposer CreateDebug(DeploymentPlatform deploymentPlatform) => Create(EnvironmentConfiguration.Development, deploymentPlatform);

    /// <summary>
    /// Gets a composer able to compose common debug object graphs.
    /// </summary>
    public static IComposer CreateRelease(DeploymentPlatform deploymentPlatform) => Create(EnvironmentConfiguration.Production, deploymentPlatform);

    private static IComposer Create(IEnvironmentConfiguration environmentConfiguration, DeploymentPlatform deploymentPlatform) => Composer.Create(c =>
    {
        c.RegisterInstance(environmentConfiguration);
        c.RegisterSingleton<IOperatingSystemInfo, OperatingSystemInfo>();
        c.RegisterSingleton<IExecutionEnvironment>(() =>
        {
            var configuration = c.GetInstance<IEnvironmentConfiguration>();
            var operatingSystem = c.GetInstance<IOperatingSystemInfo>();
            var result = new ExecutionEnvironment(
                configuration,
                operatingSystem,
                deploymentPlatform);

            return result;
        });
    });
}
