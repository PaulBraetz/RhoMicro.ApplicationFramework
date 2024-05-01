namespace RhoMicro.ApplicationFramework.Hosting;
using SimpleInjector;

/// <summary>
/// Represents a handler to invoke upon <see cref="Container"/> verification throwing an <see cref="DiagnosticVerificationException"/>.
/// </summary>
/// <param name="exception"></param>
public delegate void ContainerVerificationExceptionHandler(DiagnosticVerificationException exception);
/// <summary>
/// Contains common <see cref="ContainerVerificationExceptionHandler"/>s.
/// </summary>
public static class ContainerVerificationExceptionHandlers
{
    /// <summary>
    /// Gets a handler that will ignore all exceptions.
    /// </summary>
    public static ContainerVerificationExceptionHandler Ignore { get; } = static ex => { };
    /// <summary>
    /// Gets a handler that will rethrow all exceptions.
    /// </summary>
    public static ContainerVerificationExceptionHandler Rethrow { get; } = static ex => throw ex;
    /// <summary>
    /// Gets a handler that will terminate the application.
    /// </summary>
    public static ContainerVerificationExceptionHandler ApplicationExit { get; } = static ex =>
    {
        ArgumentNullException.ThrowIfNull(ex);

        //await integrator logs roughly
        Thread.Sleep(2500);
        Console.WriteLine(String.Concat(Enumerable.Repeat('─', 100)));
        Console.WriteLine(ex.Message);
        Environment.Exit(1);
    };
}
