namespace RhoMicro.ApplicationFramework.Common.Transactions;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

/// <summary>
/// The factory used to instantiate root and child instances of <see cref="ProgramTransaction"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="stateMachineFactory">The factory to use when instantiating new state machines for transactions to use.</param>
/// <param name="settingsFactory">The factory touse when creating new settings for new state machines.</param>
public sealed class ProgramTransactionFactory(
    IFactory<ITransactionStateMachine, ITransactionStateMachineSettings> stateMachineFactory,
    IFactory<ITransactionStateMachineSettings> settingsFactory) : ITransactionFactory<ITransaction>
{
    /// <inheritdoc/>
    public Task<ITransaction> CreateRoot(CancellationToken cancellationToken)
    {
        var stateMachine = CreateStateMachine();
        var result = new ProgramTransaction(stateMachine);

        return Task.FromResult<ITransaction>(result);
    }

    /// <inheritdoc/>
    public async Task<ITransaction> CreateChild(ITransaction parent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var stateMachine = CreateStateMachine();
        var result = new ProgramTransaction(stateMachine);
        await parent.Attach(result, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        return result;
    }

    private ITransactionStateMachine CreateStateMachine()
    {
        var settings = settingsFactory.Create();
        var stateMachine = stateMachineFactory.Create(settings);
        return stateMachine;
    }
}
