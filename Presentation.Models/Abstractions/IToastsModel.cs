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
/// Argument for <see cref="IToastsModel.ToastsChanged"/> event.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="type">The type of change that occured.</param>
/// <param name="toastAffected">The toast affected by the change.</param>
public sealed class ToastChangedEventArgs(ToastsChangedType type, IToastModel toastAffected) : EventArgs
{
    /// <summary>
    /// Gets the type of change that occured.
    /// </summary>
    public ToastsChangedType Type { get; } = type;
    /// <summary>
    /// Gets the toast affected by the change.
    /// </summary>
    public IToastModel ToastAffected { get; } = toastAffected;
}
