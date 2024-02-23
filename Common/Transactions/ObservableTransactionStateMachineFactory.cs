namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Factory for creating instances of <see cref="IObservableTransactionStateMachine"/>.
/// </summary>
public sealed class ObservableTransactionStateMachineFactory : IFactory<IObservableTransactionStateMachine, ITransactionStateMachineSettings>
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
	public static ObservableTransactionStateMachineFactory Instance { get; } = new();

    /// <inheritdoc/>
    public IObservableTransactionStateMachine Create(ITransactionStateMachineSettings parameters)
    {
        var (comparer, ignore, timeout) = parameters;
        var result = new ObservableTransactionStateMachine(comparer)
        {
            FamilySynchronizationTimeout = timeout,
            IgnoreMultipleFlushes = ignore
        };

        return result;
    }
}
