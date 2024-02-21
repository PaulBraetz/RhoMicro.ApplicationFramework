#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
namespace RhoMicro.ApplicationFramework.Common.Transactions;

using System;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Exceptions;

using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Adapter for instances of <typeparamref name="TTransaction"/> onto <see cref="IAsyncDisposable"/>, so 
/// that transactions may be used inside a <see langword="await"/> <see langword="using"/> statement.
/// </summary>
/// <typeparam name="TTransaction">The type of transaction to adapt.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="transaction">The transaction whose <see cref="ITransaction.Flush(CancellationToken)"/> to call upon disposal.</param>
public readonly struct TransactionFlushingDisposable<TTransaction>(TTransaction transaction)
    : IEquatable<TransactionFlushingDisposable<TTransaction>>
    where TTransaction : ITransaction
{
    /// <summary>
    /// Gets the adapted transaction.
    /// </summary>
    public TTransaction Transaction { get; } = transaction;
    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if(Transaction != null)
            await Transaction.Flush(default).ConfigureAwait(continueOnCapturedContext: false);
    }
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => throw Throw.NotSupportedException.TransactionFlushingDisposableEquals;
    /// <inheritdoc/>
    public Boolean Equals(TransactionFlushingDisposable<TTransaction> other) => throw Throw.NotSupportedException.TransactionFlushingDisposableEquals;
    /// <inheritdoc/>
    public override Int32 GetHashCode() => throw Throw.NotSupportedException.TransactionFlushingDisposableGetHashCode;
    /// <inheritdoc/>
    public static Boolean operator ==(TransactionFlushingDisposable<TTransaction> left, TransactionFlushingDisposable<TTransaction> right) => left.Equals(right);
    /// <inheritdoc/>
    public static Boolean operator !=(TransactionFlushingDisposable<TTransaction> left, TransactionFlushingDisposable<TTransaction> right) => !( left == right );
}