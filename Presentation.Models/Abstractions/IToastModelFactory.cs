namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Represents a factory for creating toast models.
/// </summary>
public interface IToastModelFactory
{
    /// <summary>
    /// Creates a new toast model.
    /// </summary>
    /// <param name="header">The header to be displayed.</param>
    /// <param name="body">The body to be displayed.</param>
    /// <param name="type">The toasts type.</param>
    /// <param name="lifespan">The lifespan of this toast.</param>
    /// <returns>A new toast.</returns>
    IToastModel Create(String header, String body, ToastType type, TimeSpan lifespan);
}
