namespace RhoMicro.ApplicationFramework.Hosting;
using SimpleInjector;

/// <summary>
/// Represents options to use when running apps.
/// </summary>
public sealed class AppRunOptions
{
    /// <summary>
    /// Gets or sets the logger to apply to the <see cref="Container"/> verification used before execution.
    /// </summary>
    public IContainerLogger Logger { get; set; } = CompositeContainerLogger.Default;
    /// <summary>
    /// Gets or sets the handler to invoke upon <see cref="Container"/> verification throwing an <see cref="DiagnosticVerificationException"/>.
    /// </summary>
    public ContainerVerificationExceptionHandler VerificationExceptionHandler { get; set; } = ContainerVerificationExceptionHandlers.ApplicationExit;
}
