namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Non-contextual programmatic transaction.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="stateMachine">The state machine using which to represent the internal state.</param>
public sealed partial class ProgramTransaction(ITransactionStateMachine stateMachine)
    : TransactionBase(stateMachine), ITransaction, ITransactionStateMachine
{
    /// <inheritdoc/>
    public override Task Commit(CancellationToken cancellationToken = default) =>
        StateMachine.Commit(cancellationToken);

    /// <inheritdoc/>
    public override async Task Flush(CancellationToken cancellationToken) =>
        await StateMachine.RequestFlush(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
}
