namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Represents an observable transactional unit of work.
/// </summary>
public interface IObservableTransactionStateMachine : ITransactionStateMachine
{
    /// <summary>
    /// Invoked after the state machine has been flushed while set to commit.
    /// </summary>
    event AsyncEventHandler? Committed;
    /// <summary>
    /// Invoked after the state machine has been flushed while set to rollback.
    /// </summary>
    event AsyncEventHandler? RolledBack;
}
