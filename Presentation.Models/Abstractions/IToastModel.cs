namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
/// <summary>
/// Represents a toast.
/// </summary>
public interface IToastModel
{
    /// <summary>
    /// Invoked after the toast has expired.
    /// </summary>
    event EventHandler? HasExpired;
    /// <summary>
    /// Gets a value indicating whether the toast has expired and should be hidden.
    /// </summary>
    Boolean Expired { get; }
    /// <summary>
    /// Expires the toast.
    /// </summary>
    void Expire();
    /// <summary>
    /// Gets the header to be displayed.
    /// </summary>
    String Header { get; }
    /// <summary>
    /// Gets the body to be displayed.
    /// </summary>
    String Body { get; }
    /// <summary>
    /// Gets the toasts type.
    /// </summary>
    ToastType Type { get; }
    /// <summary>
    /// Gets the lifespan of this toast.
    /// </summary>
    TimeSpan Lifespan { get; }
    /// <summary>
    /// Gets the time at which the toast was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }
}
