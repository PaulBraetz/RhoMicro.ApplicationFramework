namespace RhoMicro.ApplicationFramework.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

sealed partial class DbContextTransaction
{
    /// <summary>
    /// Factory for instances of <see cref="DbContextTransaction"/>
    /// </summary>
    /// <remarks>
    /// Initializes a new instance.
    /// </remarks>
    /// <param name="stateMachineFactory">The factory to use when instantiating new state machines for transactions to use.</param>
    /// <param name="settingsFactory">The factory to use when creating new settings for new state machines.</param>
    /// <param name="dbContext">The db context to use when creating transactions.</param>
#pragma warning disable CA1034 // Nested types should not be visible
    public sealed class Factory(
        DbContext dbContext,
        IFactory<IObservableTransactionStateMachine, ITransactionStateMachineSettings> stateMachineFactory,
        IFactory<ITransactionStateMachineSettings> settingsFactory) : ITransactionFactory<ITransaction<IDbContextTransaction>, IDbContextTransaction>
#pragma warning restore CA1034 // Nested types should not be visible
    {
        private async Task<DbContextTransaction> CreateInternal(Boolean isRoot, CancellationToken cancellationToken)
        {
            var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            var stateMachine = CreateStateMachine();
            var result = new DbContextTransaction(isRoot, transaction, stateMachine);
            await transaction.CreateSavepointAsync(result._savepointName, cancellationToken);

            stateMachine.Committed += result.OnCommitted;
            stateMachine.RolledBack += result.OnRolledBack;

            return result;
        }

        private IObservableTransactionStateMachine CreateStateMachine()
        {
            var settings = settingsFactory.Create();
            settings.IgnoreMultipleFlushes = true;
            var result = stateMachineFactory.Create(settings);

            return result;
        }

        /// <inheritdoc/>
        public async Task<ITransaction<IDbContextTransaction>> CreateChild(ITransaction parent, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(parent);

            var result = await CreateInternal(isRoot: false, cancellationToken);
            await parent.Attach(result, cancellationToken);

            return result;
        }
        /// <inheritdoc/>
        public async Task<ITransaction<IDbContextTransaction>> CreateRoot(CancellationToken cancellationToken) =>
            await CreateInternal(isRoot: true, cancellationToken);
        /// <inheritdoc/>
        public Task<ITransaction<IDbContextTransaction>> CreateChild(ITransaction<IDbContextTransaction> parent, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(parent);

            var stateMachine = CreateStateMachine();
            var result = new DbContextTransaction(false, parent.Context, stateMachine);

            return Task.FromResult<ITransaction<IDbContextTransaction>>(result);
        }
    }
}