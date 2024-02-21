namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents the model for a multicontrol that items may be added or removed to via the view.
/// </summary>
/// <typeparam name="TSubControl">The type of subcontrol encapsulated.</typeparam>
public interface IDynamicMultiControlModel<TSubControl> : IMultiControlModel<IDynamicMultiControlItemModel<TSubControl>>
{
    /// <summary>
    /// Gets the button model used for adding an item.
    /// </summary>
    IButtonModel Add { get; }
}
