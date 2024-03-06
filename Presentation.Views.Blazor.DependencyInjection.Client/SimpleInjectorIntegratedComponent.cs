namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using System.Reflection;

using Microsoft.AspNetCore.Components;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Base component that integrates with SimpleInjector.
/// </summary>
public abstract class SimpleInjectorIntegratedComponent : ComponentBase, IHandleEvent
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    protected SimpleInjectorIntegratedComponent()
    {
        var thisType = GetType();
        ComponentType = thisType.GetCustomAttribute<RenderModeProxyAttribute>() is { ComponentType: Type proxiedType }
            ? proxiedType
            : thisType.GetCustomAttribute<RenderModeWrapperAttribute>() is { ComponentType: Type wrappedType }
            ? wrappedType
            : thisType;
    }

    /// <summary>
    /// Gets the type of this component. The runtime type might differ from this type, as
    /// render mode proxy and/or wrapper component type implementations are used. 
    /// This property gets the declared and publicly used component type.
    /// </summary>
    public Type ComponentType { get; }

    /// <summary>
    /// Gets or sets the service scope applier to use when handling events.
    /// This property should not be set or used in client code.
    /// </summary>
    [Injected] public required ServiceScopeApplier Applier { get; set; }

    /// <summary>
    /// Gets the environment in which the component is being executed.
    /// </summary>
    [Injected] public required IExecutionEnvironment ExecutionEnvironment { get; set; }

    /// <inheritdoc/>
    public Task HandleEventAsync(EventCallbackWorkItem item, Object? arg)
    {
        Applier.ApplyServiceScope();

        var task = item.InvokeAsync(arg);
        var shouldAwaitTask = task.Status is not TaskStatus.RanToCompletion and not TaskStatus.Canceled;

        StateHasChanged();

        return shouldAwaitTask ?
            CallStateHasChangedOnAsyncCompletion(task) :
            Task.CompletedTask;
    }

    private async Task CallStateHasChangedOnAsyncCompletion(Task task)
    {
        try
        {
            // explicitly continue on captured context, as that context is the ui context
            await task.ConfigureAwait(continueOnCapturedContext: true);
        } catch
        {
            if(task.IsCanceled)
            {
                return;
            }

            throw;
        }

        base.StateHasChanged();
    }
}
