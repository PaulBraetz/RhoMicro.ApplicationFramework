namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Client;

using SimpleInjector;

/// <summary>
/// Helper class for integrating SimpleInjector with Blazor.
/// </summary>
public sealed class ScopeAccessor : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets or sets the current Scope.
    /// </summary>
    public Scope? Scope { get; set; }

    /// <inheritdoc/>
    public ValueTask DisposeAsync() => Scope?.DisposeAsync() ?? ValueTask.CompletedTask;

    /// <inheritdoc/>
    public void Dispose() => Scope?.Dispose();
}
