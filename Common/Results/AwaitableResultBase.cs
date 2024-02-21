namespace RhoMicro.ApplicationFramework.Common.Results;
/// <summary>
/// Base result type exposing task wrappings.
/// </summary>
/// <typeparam name="TSelf">The self type.</typeparam>
public abstract class AwaitableResultBase<TSelf>
    where TSelf : AwaitableResultBase<TSelf>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the type does not imlement <typeparamref name="TSelf"/>.</exception>
    protected AwaitableResultBase()
    {
        if(this is not TSelf)
            throw new InvalidOperationException($"Unable to construct instance of {typeof(AwaitableResultBase<TSelf>)} because it does not implement {typeof(TSelf)}.");
    }

    private Task<TSelf>? _task;

    /// <summary>
    /// Gets this instance wrapped in a <see cref="ValueTask"/>.
    /// </summary>
    public Task<TSelf> Task => _task ??= System.Threading.Tasks.Task.FromResult((TSelf)this);
}