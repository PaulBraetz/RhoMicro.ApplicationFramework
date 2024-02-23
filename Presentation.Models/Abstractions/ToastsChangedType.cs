namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Contains descriptions for the type of change having occured upon <see cref="IToastsModel.ToastsChanged"/> having been raised.
/// </summary>
public enum ToastsChangedType
{
    /// <summary>
    /// A toast has been added.
    /// </summary>
    Added,
    /// <summary>
    /// A toast has been removed.
    /// </summary>
    Removed
}
