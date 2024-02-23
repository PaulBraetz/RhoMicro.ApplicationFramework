namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="INavigationTreeChildNode"/>.
/// </summary>
public sealed class NavigationTreeChildNode : NavigationTreeNode, INavigationTreeChildNode
{
    internal NavigationTreeChildNode(String name, String path, INavigationTreeNode parent)
        : base(name, path)
        => Parent = parent;
    /// <inheritdoc/>
    public INavigationTreeNode Parent { get; }
}
