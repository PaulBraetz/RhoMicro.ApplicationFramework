namespace RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;
/// <summary>
/// Represents a transaction manager.
/// </summary>
public interface ITransactionFactory<TTransaction>
    where TTransaction : ITransaction
{
    /// <summary>
    /// Creates a root transaction
    /// </summary>
    /// <param name="cancellationToken">The token used to signal cancellation.</param>
    /// <returns>
    /// A task that upon completion contains a new root transaction.
    /// </returns>
    Task<TTransaction> CreateRoot(CancellationToken cancellationToken);
    /// <summary>
    /// Creates a child transaction and attaches it to its parent.
    /// </summary>
    /// <param name="parent">The parent transaction to attach the newly created child to.</param>
    /// <param name="cancellationToken">The token used to signal cancellation.</param>
    /// <returns>
    /// A task that upon completion contains the new child transaction.
    /// </returns>
    Task<TTransaction> CreateChild(ITransaction parent, CancellationToken cancellationToken);
}

/// <summary>
/// Represents a transaction manager.
/// </summary>
public interface ITransactionFactory<TTransaction, TContext> : ITransactionFactory<TTransaction>
    where TTransaction : ITransaction<TContext>
{
    /// <summary>
    /// Creates a child transaction and attaches it to its parent, taking into account the parents context.
    /// </summary>
    /// <param name="parent">The parent transaction to attach the newly created child to.</param>
    /// <param name="cancellationToken">The token used to signal cancellation.</param>
    /// <returns>
    /// A task that upon completion contains the new child transaction.
    /// </returns>
    Task<TTransaction> CreateChild(ITransaction<TContext> parent, CancellationToken cancellationToken);
}
