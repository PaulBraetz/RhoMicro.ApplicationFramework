namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Client;

using Microsoft.Extensions.DependencyInjection;

using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;
using SimpleInjector.Lifestyles;

/// <summary>
/// Helper class for applying the request scope to the scope accessor.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="serviceScope">The container with which to integrate.</param>
/// <param name="accessor">The accessor whose scope to apply.</param>
/// <param name="container">The container which to integrate.</param>
public sealed class ServiceScopeApplier(IServiceScope serviceScope, ScopeAccessor accessor, Container container)
{
    private static readonly AsyncScopedLifestyle _lifestyle = new();

    /// <summary>
    /// Applies the ambient scope to the accessor.
    /// </summary>
    public void ApplyServiceScope()
    {
        if(accessor.Scope is null)
        {
            var scope = AsyncScopedLifestyle.BeginScope(container);

            accessor.Scope = scope;

            scope.GetInstance<ServiceScopeProvider>().ServiceScope = serviceScope;
        } else
        {
            _lifestyle.SetCurrentScope(accessor.Scope);
        }
    }
}
