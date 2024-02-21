namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Default implementation of <see cref="ITransactionStateMachine"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="comparer">The comparer used to determine children equality.</param>
public class TransactionStateMachine(IEqualityComparer<ITransactionStateMachine> comparer) : DisposableBase, ITransactionStateMachine
{
    /// <summary>
    /// Gets or sets a value indicating whether to ignore multiple flushes.
    /// </summary>
    public Boolean IgnoreMultipleFlushes { get; set; }

    /// <summary>
    /// Gets or sets the timeout delay when synchronizing access to 
    /// <see cref="AddChild(ITransactionStateMachine, CancellationToken)"/>,
    /// <see cref="RemoveChild(ITransactionStateMachine, CancellationToken)"/>,
    /// <see cref="SetParent(ITransactionStateMachine, CancellationToken)"/> and
    /// <see cref="RemoveParent(CancellationToken)"/>.
    /// </summary>
    public TimeSpan FamilySynchronizationTimeout { get; set; } = TimeSpan.FromSeconds(5);

    private const String _familyTimeoutMessage = "Timeout while attempting to synchronize transaction family creation. This could indicate a failure to override Equals and GetHashCode when implementing ITransactionStateMachine through composition and then attempting to add an implementation instance as a child to a transaction state machine.";
    private readonly SemaphoreSlim _gate = new(1);
    private readonly HashSet<ITransactionStateMachine> _children = new(comparer);

    private ITransactionStateMachine? _parent;
    private Boolean _isCommit;

    /// <inheritdoc/>
    public Object Identity => this;
    /// <inheritdoc/>
    public TransactionState State { get; private set; }

    /// <inheritdoc/>
    public async ValueTask AddChild(ITransactionStateMachine child, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(child);

        if(EqualityComparer<ITransactionStateMachine>.Default.Equals(this, child))
            throw new InvalidOperationException("Unable to add self reference to children.");

        if(_children.Contains(child))
            return;

        EnsureMutableSelf("add child");
        EnsureMutableChild(child, "add");

        using var _0 = await WaitFamily(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        if(_children.Contains(child))
            return;

        EnsureMutableSelf("add child");
        EnsureMutableChild(child, "add");

        _ = _children.Add(child);
        await child.SetParent(this, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    /// <inheritdoc/>
    public async ValueTask RemoveChild(ITransactionStateMachine child, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(child);

        if(!_children.Contains(child))
            return;

        EnsureMutableSelf("add child");
        EnsureMutableChild(child, "remove");

        using var _0 = await WaitFamily(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if(!_children.Contains(child))
            return;

        EnsureMutableSelf("add child");
        EnsureMutableChild(child, "remove");

        _ = _children.Remove(child);
        await child.RemoveParent(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    /// <inheritdoc/>
    public async ValueTask SetParent(ITransactionStateMachine parent, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parent);

        if(EqualityComparer<ITransactionStateMachine>.Default.Equals(this, parent))
            throw new InvalidOperationException("Unable to set parent to self reference.");

        if(EqualityComparer<ITransactionStateMachine>.Default.Equals(_parent, parent))
            return;

        EnsureMutableSelf("set parent");
        EnsureMutableParent(parent, "set");

        using var _0 = await WaitFamily(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if(EqualityComparer<ITransactionStateMachine>.Default.Equals(_parent, parent))
            return;

        EnsureMutableSelf("set parent");
        EnsureMutableParent(parent, "set");

        _parent = parent;
        await parent.AddChild(this, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    /// <inheritdoc/>
    public async ValueTask RemoveParent(CancellationToken cancellationToken = default)
    {
        if(_parent == null)
            return;

        EnsureMutableSelf("remove parent");
        EnsureMutableParent(_parent, "set");

        using var _0 = await WaitFamily(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if(_parent == null)
            return;

        EnsureMutableSelf("remove parent");
        EnsureMutableParent(_parent, "set");

        var parent = _parent;
        _parent = null;
        await parent.RemoveChild(this, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async ValueTask<IDisposable> WaitFamily(CancellationToken cancellationToken = default)
    {
        var entered = await _gate.WaitAsync(FamilySynchronizationTimeout, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        var result = entered ?
            new CallbackDisposable(() => _gate.Release()) :
            throw new TimeoutException(_familyTimeoutMessage);

        return result;
    }

    private void EnsureMutableSelf(String action) =>
        EnsureMutableFamilyMember(this, $"{action} while");
    private static void EnsureMutableChild(ITransactionStateMachine child, String action) =>
        EnsureMutableFamilyMember(child, $"{action} child that is");
    private static void EnsureMutableParent(ITransactionStateMachine parent, String action) =>
        EnsureMutableFamilyMember(parent, $"{action} parent that is");
    private static void EnsureMutableFamilyMember(ITransactionStateMachine member, String action)
    {
        if(member.State is not TransactionState.Mutable)
            throw new InvalidOperationException($"Unable to {action} in the '{member.State}' state.");
    }

    /// <inheritdoc/>
    public async Task Commit(CancellationToken cancellationToken = default)
    {
        using var _0 = await _gate.WaitDisposableAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if(State is not TransactionState.Mutable)
            throw new InvalidOperationException($"Unable to commit while in the '{State}' state.");

        _isCommit = true;
    }
    /// <inheritdoc/>
    public async Task Rollback(CancellationToken cancellationToken = default)
    {
        using var _0 = await _gate.WaitDisposableAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if(State is not TransactionState.Mutable)
            throw new InvalidOperationException($"Unable to rollback while in the '{State}' state.");

        _isCommit = false;
    }

    /// <inheritdoc/>
    public async ValueTask RequestFlush(CancellationToken cancellationToken = default)
    {
        using var _0 = await _gate.WaitDisposableAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        if(State > TransactionState.Immutable)
            return;

        State = TransactionState.Immutable;

        if(_parent == null)
        {
            var forceRollback = !await GetIsCommitInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            await FlushInternal(forceRollback, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
    /// <inheritdoc/>
    public async Task Flush(Boolean forceRollback, CancellationToken cancellationToken = default)
    {
        using var _0 = await _gate.WaitDisposableAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        await FlushInternal(forceRollback, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async ValueTask FlushInternal(Boolean forceRollback, CancellationToken cancellationToken = default)
    {
        if(State > TransactionState.Immutable)
        {
            if(IgnoreMultipleFlushes)
                return;

            throw new InvalidOperationException($"Unable to flush while in the '{State}' state.");
        }

        if(forceRollback)
            _isCommit = false;

        try
        {
            var forceChildrenRollback = !_isCommit;
            var childFlushes = _children.Select(c => c.Flush(forceChildrenRollback, cancellationToken));
            await Task.WhenAll(childFlushes).ConfigureAwait(continueOnCapturedContext: false);

            var isConsistent = _children.Count == 0;
            foreach(var child in _children)
            {
                isConsistent = child.State == TransactionState.Flushed &&
                               _isCommit == await child.GetIsCommit(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                if(!isConsistent)
                    break;
            }

            State = isConsistent ?
                TransactionState.Flushed :
                TransactionState.FlushedWithInconsistentCommits;

            var action = _isCommit ?
                OnCommit(cancellationToken) :
                OnRollback(cancellationToken);

            await action.ConfigureAwait(continueOnCapturedContext: false);
        } catch
        {
            State = TransactionState.Faulted;
            throw;
        }
    }

    /// <inheritdoc/>
    public async ValueTask<Boolean> GetIsCommit(CancellationToken cancellationToken = default)
    {
        using var _0 = await _gate.WaitDisposableAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        var result = await GetIsCommitInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return result;
    }

    private async ValueTask<Boolean> GetIsCommitInternal(CancellationToken cancellationToken)
    {
        if(!_isCommit)
            return false;

        foreach(var child in _children)
        {
            var childIsCommit = await child.GetIsCommit(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            if(!childIsCommit)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Invoked during <see cref="Flush(Boolean, CancellationToken)"/> or <see cref="RequestFlush(CancellationToken)"/>
    /// if the transaction is set to commit.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    protected virtual Task OnCommit(CancellationToken cancellationToken) => Task.CompletedTask;
    /// <summary>
    /// Invoked during <see cref="Flush(Boolean, CancellationToken)"/> or <see cref="RequestFlush(CancellationToken)"/>
    /// if the transaction is not set to commit.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    protected virtual Task OnRollback(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc/>
    public override String ToString()
    {
        var action = _isCommit ?
            "Commit" :
            "Rollback";
        var result = $"{State} {action}";

        return result;
    }
    /// <inheritdoc/>
    protected override void DisposeManaged() => _gate.Dispose();
}