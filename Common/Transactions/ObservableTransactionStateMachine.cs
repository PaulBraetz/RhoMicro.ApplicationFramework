namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Default implementation of <see cref="IObservableTransactionStateMachine"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="comparer">The comparer used to determine children equality.</param>
public sealed class ObservableTransactionStateMachine(IEqualityComparer<ITransactionStateMachine> comparer)
    : TransactionStateMachine(comparer), IObservableTransactionStateMachine
{

    /// <inheritdoc/>
    public event AsyncEventHandler? Committed;
    /// <inheritdoc/>
    public event AsyncEventHandler? RolledBack;

    /// <inheritdoc/>
    protected override Task OnCommit(CancellationToken cancellationToken) =>
        Committed.InvokeAsync(this, cancellationToken);
    /// <inheritdoc/>
    protected override Task OnRollback(CancellationToken cancellationToken) =>
        RolledBack.InvokeAsync(this, cancellationToken);
}
