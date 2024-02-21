namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Models a relative or absolute path.
/// </summary>
public interface IPathModel
{
    /// <summary>
    /// Gets a value indicating whether the path is absolute.
    /// </summary>
    Boolean IsAbsolute { get; }
    /// <summary>
    /// Gets the path.
    /// </summary>
    String Path { get; }
}
