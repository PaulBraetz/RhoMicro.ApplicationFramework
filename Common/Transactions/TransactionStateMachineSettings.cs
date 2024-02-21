namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Default implementation of <see cref="ITransactionStateMachineSettings"/>.
/// </summary>
public sealed class TransactionStateMachineSettings : ITransactionStateMachineSettings
{
    /// <inheritdoc/>
    public IEqualityComparer<ITransactionStateMachine> Comparer { get; set; } =
        TransactionStateMachineEqualityComparer.Instance;
    /// <inheritdoc/>
    public Boolean IgnoreMultipleFlushes { get; set; }
    /// <inheritdoc/>
    public TimeSpan FamilySynchronizationTimeout { get; set; } =
        TimeSpan.FromSeconds(5);
}
