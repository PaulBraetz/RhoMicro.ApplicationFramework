namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a model for a container containing multiple subcontrols.
/// </summary>
/// <typeparam name="TSubControlModel">The type of subcontrol model encapsulated.</typeparam>
public interface IMultiControlModel<TSubControlModel>
{
    /// <summary>
    /// Gets or sets the models label.
    /// </summary>
    String Label { get; set; }
    /// <summary>
    /// Gets or sets the models description.
    /// </summary>
    String Description { get; set; }
    /// <summary>
    /// Gets the subcontrols to display.
    /// </summary>
    ICollection<TSubControlModel> Items { get; }
}
