namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Common.Transactions</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adapts the transaction onto the <see cref="IAsyncDisposable"/> interface, such that
    /// the resulting instance will flush the transaction upon disposal.
    /// </summary>
    /// <typeparam name="TTransaction">The type of transaction to adapt.</typeparam>
    /// <param name="transaction">The transaction whose <see cref="ITransaction.Flush(CancellationToken)"/> to call upon disposing the result.</param>
    /// <returns>A new adapter that will call the transactions <see cref="ITransaction.Flush(CancellationToken)"/> method upon being disposed.</returns>
    public static TransactionFlushingDisposable<TTransaction> AsAutoFlush<TTransaction>(this TTransaction transaction)
        where TTransaction : ITransaction
    {
        var result = new TransactionFlushingDisposable<TTransaction>(transaction);

        return result;
    }
    /// <summary>
    /// Deconstructs an instance of <see cref="ITransactionStateMachineSettings"/>.
    /// </summary>
    /// <param name="settings">The instance to deconstruct.</param>
    /// <param name="comparer">The comparer used to determine children equality.</param>
    /// <param name="ignoreMultipleFlushes">A value indicating whether to ignore multiple flushes.</param>
    /// <param name="familySynchronizationTimeout">
    /// The timeout delay when synchronizing access to 
    /// <see cref="ITransactionStateMachine.AddChild(ITransactionStateMachine, CancellationToken)"/>,
    /// <see cref="ITransactionStateMachine.RemoveChild(ITransactionStateMachine, CancellationToken)"/>,
    /// <see cref="ITransactionStateMachine.SetParent(ITransactionStateMachine, CancellationToken)"/> and
    /// <see cref="ITransactionStateMachine.RemoveParent(CancellationToken)"/>.
    /// </param>
    public static void Deconstruct(
        this ITransactionStateMachineSettings settings,
        out IEqualityComparer<ITransactionStateMachine> comparer,
        out Boolean ignoreMultipleFlushes,
        out TimeSpan familySynchronizationTimeout)
    {
        ArgumentNullException.ThrowIfNull(settings);

        comparer = settings.Comparer;
        ignoreMultipleFlushes = settings.IgnoreMultipleFlushes;
        familySynchronizationTimeout = settings.FamilySynchronizationTimeout;
    }
}