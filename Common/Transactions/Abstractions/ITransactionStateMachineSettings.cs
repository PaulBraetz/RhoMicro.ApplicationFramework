namespace RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

using System;
using System.Collections.Generic;

/// <summary>
/// Settings for configuring the creation of transaction state machines.
/// </summary>
public interface ITransactionStateMachineSettings
{
    /// <summary>
    /// Gets or sets the comparer used to determine children equality.
    /// </summary>
    IEqualityComparer<ITransactionStateMachine> Comparer { get; set; }
    /// <summary>
    /// Gets or sets the value indicating whether to ignore multiple flushes.
    /// </summary>
    Boolean IgnoreMultipleFlushes { get; set; }
    /// <summary>
    /// Gets  or sets the timeout delay when synchronizing access to 
    /// <see cref="ITransactionStateMachine.AddChild(ITransactionStateMachine, CancellationToken)"/>,
    /// <see cref="ITransactionStateMachine.RemoveChild(ITransactionStateMachine, CancellationToken)"/>,
    /// <see cref="ITransactionStateMachine.SetParent(ITransactionStateMachine, CancellationToken)"/> and
    /// <see cref="ITransactionStateMachine.RemoveParent(CancellationToken)"/>.
    /// </summary>
    TimeSpan FamilySynchronizationTimeout { get; set; }
}
