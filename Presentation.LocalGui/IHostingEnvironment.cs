namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

/// <summary>
/// Represents the environment in which an application is being run.
/// </summary>
public interface IHostingEnvironment
{
    /// <summary>
    /// Gets the root path of application content files.
    /// </summary>
    String ContentRootPath { get; }
    /// <summary>
    /// Gets the name of the environment.
    /// </summary>
    String EnvironmentName { get; }
}
