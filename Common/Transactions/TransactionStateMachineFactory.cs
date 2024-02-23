namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Factory for creating instances of <see cref="ITransactionStateMachine"/>.
/// </summary>
public sealed class TransactionStateMachineFactory : IFactory<ITransactionStateMachine, ITransactionStateMachineSettings>
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
	public static TransactionStateMachineFactory Instance { get; } = new();

    /// <inheritdoc/>
    public ITransactionStateMachine Create(ITransactionStateMachineSettings parameters)
    {
        var (comparer, ignore, timeout) = parameters;
        var result = new TransactionStateMachine(comparer)
        {
            FamilySynchronizationTimeout = timeout,
            IgnoreMultipleFlushes = ignore
        };

        return result;
    }
}
