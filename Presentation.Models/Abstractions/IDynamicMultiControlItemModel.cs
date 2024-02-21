namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents an item contained in a dynamic multicontrol.
/// </summary>
/// <typeparam name="TSubControlModel">The type of subcontrol encapsulated.</typeparam>
public interface IDynamicMultiControlItemModel<TSubControlModel>
{
    /// <summary>
    /// Gets the subcontrol do display.
    /// </summary>
    TSubControlModel Control { get; }
    /// <summary>
    /// Gets the button model using which the item may be removed from its parent control.
    /// </summary>
    IButtonModel Remove { get; }
}
