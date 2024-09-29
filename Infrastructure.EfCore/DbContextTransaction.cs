namespace RhoMicro.ApplicationFramework.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// Db context transaction wrapper for instances of <see cref="DbContext"/>.
/// </summary>
public sealed partial class DbContextTransaction : TransactionBase<IDbContextTransaction>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="isRoot">Indicates whether this instance is a root transaction.</param>
    /// <param name="transaction">The underlying db context transaction.</param>
    /// <param name="stateMachine">The state machine to use for managing the transactional state.</param>
    public DbContextTransaction(Boolean isRoot, IDbContextTransaction transaction, ITransactionStateMachine stateMachine)
        : base(stateMachine, transaction)
    {
        _isRoot = isRoot;
        _savepointName = $"Savepoint_Transaction_{Name}";
    }

    private async Task OnCommitted(Object? _, IAsyncEventArguments args)
    {
        await Context.ReleaseSavepointAsync(_savepointName, args.CancellationToken);

        if(_isRoot)
        {
            await Context.CommitAsync(args.CancellationToken);
        }
    }

    private Task OnRolledBack(Object? _, IAsyncEventArguments args) =>
        Context.RollbackToSavepointAsync(_savepointName, args.CancellationToken);

    private readonly Boolean _isRoot;
    private readonly String _savepointName;

    /// <inheritdoc/>
    public override async Task Flush(CancellationToken cancellationToken)
    {
        try
        {
            await ((ITransactionStateMachine)this).Flush(forceRollback: false, default);
        } finally
        {
            if(_isRoot)
            {
                await Context.DisposeAsync();
            }
        }
    }
    /// <inheritdoc/>
    public override String ToString()
    {
        var rootString = _isRoot ? " (root)" : String.Empty;
        return $"{Name}{rootString}: {StateMachine}";
    }
}