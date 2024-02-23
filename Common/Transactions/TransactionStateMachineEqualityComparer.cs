namespace RhoMicro.ApplicationFramework.Common.Transactions;

using System.Diagnostics.CodeAnalysis;

using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Reference-Identity-based equality comparer for transactional state machines.
/// </summary>
public sealed class TransactionStateMachineEqualityComparer : IEqualityComparer<ITransactionStateMachine>
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
	public static TransactionStateMachineEqualityComparer Instance { get; } = new();

    /// <inheritdoc/>
    public Boolean Equals(ITransactionStateMachine? x, ITransactionStateMachine? y)
    {
        var result =
            x == null
            ? y == null
            : y != null && y.Identity == x.Identity;

        return result;
    }
    /// <inheritdoc/>
    public Int32 GetHashCode([DisallowNull] ITransactionStateMachine obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var result = obj.Identity.GetHashCode();

        return result;
    }
}
