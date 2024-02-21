namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a node in the route navigation tree.
/// </summary>
public interface INavigationTreeChildNode : INavigationTreeNode
{
    /// <summary>
    /// Gets the parent to this node.
    /// </summary>
    INavigationTreeNode Parent { get; }
}
