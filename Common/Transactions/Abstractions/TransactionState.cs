namespace RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;
/// <summary>
/// Represents the state a transactional state machine can be in.
/// </summary>
public enum TransactionState
{
    /// <summary>
    /// The state machine is mutable and may be committed or rolled back.
    /// </summary>
    Mutable,
    /// <summary>
    /// The state machine is immutable and waiting to be flushed.
    /// </summary>
    Immutable,
    /// <summary>
    /// The state machine is flushed successfully and all child transactions have maintained integrity.
    /// </summary>
    Flushed,
    /// <summary>
    /// The state machine is flushed successfully but at least one child is either also flushed
    /// with inconsistent commits or not conforming to the transaction trees 'IsComit'-state.
    /// </summary>
    FlushedWithInconsistentCommits,
    /// <summary>
    /// The state machne has experienced an exception while flushing.
    /// </summary>
    Faulted
}
