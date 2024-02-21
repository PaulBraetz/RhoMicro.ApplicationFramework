namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="INavigationTreeNode"/>.
/// </summary>
public class NavigationTreeNode : INavigationTreeNode
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="path">The path relative to the nodes parent.</param>
    /// <param name="name">The name of this node.</param>
    protected NavigationTreeNode(String name, String path)
    {
        Path = path;
        Name = name;
    }

    /// <summary>
    /// The default name for root nodes.
    /// </summary>
    public const String ROOT_NAME = "root";
    /// <summary>
    /// The default path for root nodes.
    /// </summary>
    public const String ROOT_PATH = "";
    /// <summary>
    /// Creates a root node.
    /// </summary>
    /// <param name="name">The name of the root node.</param>
    /// <param name="path">The path of the root node.</param>
    /// <returns>The root node.</returns>
    public static NavigationTreeNode CreateRoot(String name = ROOT_NAME, String path = ROOT_PATH)
    {
        var result = new NavigationTreeNode(name, path);

        return result;
    }
    /// <summary>
    /// Adds a child to the node.
    /// </summary>
    /// <param name="name">The name of the child node.</param>
    /// <param name="path">The path of the child node, relative to this instance.</param>
    /// <returns>The child node created.</returns>
    public NavigationTreeChildNode AddChild(String name, String path)
    {
        var child = new NavigationTreeChildNode(name, path, this);
        _children.Add(name, child);

        return child;
    }

    private readonly Dictionary<String, INavigationTreeChildNode> _children = new();

    /// <inheritdoc/>
    public String Path { get; }
    /// <inheritdoc/>
    public String Name { get; }
    /// <inheritdoc/>
    public IEnumerable<INavigationTreeChildNode> GetChildren()
    {
        var result = _children.Values;

        return result;
    }
    /// <inheritdoc/>
    public INavigationTreeChildNode this[String name]
    {
        get
        {
            var result = _children.TryGetValue(name, out var child) ?
            child :
            throw new ArgumentException($"No child named '{name}' could be located", nameof(name));

            return result;
        }
    }
    /// <inheritdoc/>
    public override String ToString() =>
        $"{{ Name = {Name}, Path = {Path}, Children = \n[\n{String.Join(",\n", _children.Values)}\n] }}";
}
