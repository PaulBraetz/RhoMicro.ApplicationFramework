namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IPathModel"/>.
/// </summary>
/// <param name="IsAbsolute">Indicates whether the path is absolute.</param>
/// <param name="Path">The path.</param>
public readonly record struct PathModel(Boolean IsAbsolute, String Path) : IPathModel 
{
    /// <summary>
    /// Creates a new absolute path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A new absolute path.</returns>
    public static IPathModel Absolute(String path) => new PathModel(true, path);
    /// <summary>
    /// Creates a new relative path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>A new relative path.</returns>
    public static IPathModel Relative(String path) => new PathModel(false, path);
}
