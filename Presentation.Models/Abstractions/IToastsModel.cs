namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Observabe toast collection.
/// </summary>
public interface IToastsModel
{
    /// <summary>
    /// Gets the current toasts to display.
    /// </summary>
    ICollection<IToastModel> GetToasts();
    /// <summary>
    /// Invoked when a toast has either been raised or expired.
    /// </summary>
    event EventHandler<ToastChangedEventArgs>? ToastsChanged;
}
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
/// <summary>
/// Argument for <see cref="IToastsModel.ToastsChanged"/> event.
/// </summary>
/// <param name="Type">The type of change that occured.</param>
/// <param name="ToastAffected">The toast affected by the change.</param>
public sealed record ToastChangedEventArgs(ToastsChangedType Type, IToastModel ToastAffected);
