namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IToastModelFactory"/>.
/// </summary>
public sealed class ToastModelFactory : IToastModelFactory
{
    /// <inheritdoc/>
    public IToastModel Create(String header, String body, ToastType type, TimeSpan lifespan)
    {
        var result = ToastModel.Create(header, body, type, lifespan);

        return result;
    }
}
