namespace RhoMicro.ApplicationFramework.Common.Environment;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Common.Environment</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Gets a value indicating whether the application has been deployed to a desktop environment.
    /// </summary>
    /// <param name="environment"></param>
    /// <returns>
    /// <see langword="true"/> if <see cref="IExecutionEnvironment.DeploymentPlatform"/> is <see cref="DeploymentPlatform.Desktop"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean IsDesktop(this IExecutionEnvironment environment) =>
        environment.Is(DeploymentPlatform.Desktop);

    /// <summary>
    /// Gets a value indicating whether the application has been deployed to a web client environment.
    /// </summary>
    /// <param name="environment"></param>
    /// <returns>
    /// <see langword="true"/> if <see cref="IExecutionEnvironment.DeploymentPlatform"/> is <see cref="DeploymentPlatform.Client"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean IsClient(this IExecutionEnvironment environment) =>
        environment.Is(DeploymentPlatform.Client);

    /// <summary>
    /// Gets a value indicating whether the application has been deployed to a web server environment.
    /// </summary>
    /// <param name="environment"></param>
    /// <returns>
    /// <see langword="true"/> if <see cref="IExecutionEnvironment.DeploymentPlatform"/> is <see cref="DeploymentPlatform.Server"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean IsServer(this IExecutionEnvironment environment) =>
        environment.Is(DeploymentPlatform.Server);

    private static Boolean Is(this IExecutionEnvironment environment, DeploymentPlatform platform)
    {
        ArgumentNullException.ThrowIfNull(environment);

        return environment.DeploymentPlatform == platform;
    }
}