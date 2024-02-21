namespace RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;
/// <summary>
/// Represents a generalized nestable transaction.
/// </summary>
public interface ITransaction
{
    /// <summary>
    /// Commits the transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    Task Commit(CancellationToken cancellationToken);
    /// <summary>
    /// Flushes the transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    Task Flush(CancellationToken cancellationToken);
    /// <summary>
    /// Attaches to the transaction a child transaction.
    /// </summary>
    /// <param name="child">The child transaction to add to this transaction.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation.</param>
    /// <returns></returns>
    ValueTask Attach(ITransactionStateMachine child, CancellationToken cancellationToken);
}
/// <summary>
/// Generic transaction providing a context to perform work on. 
/// </summary>
/// <typeparam name="TContext">The type of context to perform transactional work on.</typeparam>
public interface ITransaction<TContext> : ITransaction
{
    /// <summary>
    /// Gets the context to perform transactional work on.
    /// </summary>
    TContext Context { get; }
}