namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Base component that integrates with SimpleIjector.
/// </summary>
public abstract class SimpleInjectorIntegratedComponent : ComponentBase, IHandleEvent
{
    /// <summary>
    /// Gets or sets the service scope applier to use when handling events.
    /// This property should not be set or used in client code.
    /// </summary>
#pragma warning disable CS8618 // Should never be null during lifetime of instance due to DI
    [Injected] public ServiceScopeApplier Applier { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
