namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using Microsoft.AspNetCore.SignalR;

using SimpleInjector;

/// <summary>
/// Hub activator for integrating SimpleInjector into Blazor.
/// </summary>
/// <typeparam name="T">The type of hub to activate.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="applier">The ambient scope applier to use.</param>
/// <param name="container">The container using which to resolve the hub.</param>
public sealed class SimpleInjectorBlazorHubActivator<T>(ServiceScopeApplier applier, Container container)
    : IHubActivator<T> where T : Hub
{
    /// <inheritdoc/>
    public T Create()
    {
        applier.ApplyServiceScope();
        var result = container.GetInstance<T>();
        return result;
    }
    /// <inheritdoc/>
    public void Release(T hub) { }
}
