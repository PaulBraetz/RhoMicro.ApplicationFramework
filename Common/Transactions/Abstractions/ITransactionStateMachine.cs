namespace RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;
/// <summary>
/// Represents a transactional unit of work.
/// </summary>
public interface ITransactionStateMachine
{
    /// <summary>
    /// Gets the reference identity of the transaction.
    /// </summary>
    Object Identity { get; }
    /// <summary>
    /// Gets the state machines current state.
    /// </summary>
    TransactionState State { get; }

    /// <summary>
    /// Adds a child transaction to the transaction and sets the childs parent to this transaction.
    /// </summary>
    /// <param name="child">The nested transaction to add.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    ValueTask AddChild(ITransactionStateMachine child, CancellationToken cancellationToken = default);
    /// <summary>
    /// Removes a child transaction from the transaction and removes the childs parent.
    /// </summary>
    /// <param name="child">The nested transaction to remove.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    ValueTask RemoveChild(ITransactionStateMachine child, CancellationToken cancellationToken = default);
    /// <summary>
    /// Sets the transactions parent and adds the transaction to the parents children.
    /// </summary>
    /// <param name="parent">The transaction's new parent.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    ValueTask SetParent(ITransactionStateMachine parent, CancellationToken cancellationToken = default);
    /// <summary>
    /// Removes the transactions parent and removes the transaction from the parents children.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    ValueTask RemoveParent(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the transaction to commit.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    Task Commit(CancellationToken cancellationToken = default);
    /// <summary>
    /// Sets the transaction to rollback (the default state).
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    Task Rollback(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a value indicating whether this transaction (recursively) is to be seen as set to commit.
    /// The value returned takes into consideration the transaction's children, and theirs -recursively-.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns>
    /// <see langword="true"/> if this transaction and all of its children are
    /// set to commit; otherwise, <see langword="false"/>.
    /// </returns>
    ValueTask<Boolean> GetIsCommit(CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests the transaction to flush.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    ValueTask RequestFlush(CancellationToken cancellationToken = default);
    /// <summary>
    /// Flushes the transaction, optionally overriding its internal commit set state.
    /// </summary>
    /// <param name="forceRollback">
    /// If set to <see langword="true"/>, indicates the transaction to ignore
    /// its internal commit set state and rollback even if set to commit.
    /// If set to <see langword="false"/>, the internal state will be respected instead.
    /// </param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    Task Flush(Boolean forceRollback, CancellationToken cancellationToken = default);
}
