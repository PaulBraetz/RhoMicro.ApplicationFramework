namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Factory for creating instances of <see cref="IObservableTransactionStateMachine"/>.
/// </summary>
public sealed class ObservableTransactionStateMachineFactory : IFactory<IObservableTransactionStateMachine, ITransactionStateMachineSettings>
{
    /// <summary>
    /// Initializes a new instance. If possible, use <see cref="Instance"/> instead.
    /// </summary>
    public ObservableTransactionStateMachineFactory() { }
    /// <summary>
    /// Gets the singleton instance.
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
