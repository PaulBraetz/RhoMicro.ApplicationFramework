namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a route navigation tree node.
/// </summary>
public interface INavigationTreeNode
{
    /// <summary>
    /// Gets the path relative to the nodes parent.
    /// </summary>
    String Path { get; }
    /// <summary>
    /// Gets the name of this node.
    /// </summary>
    String Name { get; }
    /// <summary>
    /// Gets all children to this node.
    /// </summary>
    /// <returns>An enumeration of children to this node.</returns>
    IEnumerable<INavigationTreeChildNode> GetChildren();
    /// <summary>
    /// Gets the child with a specific name.
    /// </summary>
    /// <param name="name">The name of the child to get.</param>
    /// <returns>The child with the name provided.</returns>
    INavigationTreeChildNode this[String name] { get; }
}
