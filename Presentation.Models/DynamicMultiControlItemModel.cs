namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IDynamicMultiControlItemModel{TSubControl}"/>.
/// </summary>
/// <typeparam name="TSubControlModel">The type of subcontrol encapsulated.</typeparam>
public sealed class DynamicMultiControlItemModel<TSubControlModel> : IDynamicMultiControlItemModel<TSubControlModel>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="control">The items subcontrol model.</param>
    /// <param name="remove">The button used to remove the item from its parent.</param>
    public DynamicMultiControlItemModel(TSubControlModel control, IButtonModel remove)
    {
        Control = control;
        Remove = remove;
        Remove.Label = "-";
    }

    /// <inheritdoc/>
    public TSubControlModel Control { get; }
    /// <inheritdoc/>
    public IButtonModel Remove { get; }
}
