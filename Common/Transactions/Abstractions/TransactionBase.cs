#pragma warning disable CA1033 // Interface methods should be callable by child types
namespace RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;
/// <summary>
/// Base class for types implementing <see cref="ITransaction{TContext}"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="stateMachine">The state machine using which to represent the internal state.</param>
/// <param name="context">The context to perform transactional work on.</param>
/// <param name="name">The name of the transaction, or <see langword="null"/> if a default name is to be used.</param>
public abstract class TransactionBase<TContext>(ITransactionStateMachine stateMachine, TContext context, String? name = null) : TransactionBase(stateMachine, name), ITransaction<TContext>
{

    /// <inheritdoc/>
    public TContext Context { get; } = context;
}
/// <summary>
/// Base class for types implementing <see cref="ITransaction"/>.
/// </summary>
public abstract class TransactionBase : ITransaction, ITransactionStateMachine
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="stateMachine">The state machine using which to represent the internal state.</param>
    /// <param name="name">The name of the transaction, or <see langword="null"/> if a default name is to be used.</param>
    protected TransactionBase(ITransactionStateMachine stateMachine, String? name = null)
    {
        StateMachine = stateMachine;

        if(name == null)
        {
            var id = Interlocked.Increment(ref _id);
            name = $"{GetType().Name}_{id}";
        }

        Name = name;
    }

    private static Int32 _id;

    /// <summary>
    /// Gets the name of this transaction.
    /// </summary>
    protected String Name { get; }
    /// <summary>
    /// Gets the state machine used to manage the transaction.
    /// </summary>
    protected ITransactionStateMachine StateMachine { get; }

    /// <inheritdoc/>
    public override String ToString() => $"{Name}: {StateMachine}";

    /// <inheritdoc/>
    public virtual Task Commit(CancellationToken cancellationToken) => StateMachine.Commit(cancellationToken);
    /// <inheritdoc/>
    public abstract Task Flush(CancellationToken cancellationToken);
    /// <inheritdoc/>
    public ValueTask Attach(ITransactionStateMachine child, CancellationToken cancellationToken) =>
        StateMachine.AddChild(child, cancellationToken);

    #region ITransactionStateMachine implementation
    Object ITransactionStateMachine.Identity => StateMachine.Identity;
    TransactionState ITransactionStateMachine.State => StateMachine.State;

    ValueTask ITransactionStateMachine.AddChild(ITransactionStateMachine child, CancellationToken cancellationToken) => StateMachine.AddChild(child, cancellationToken);
    ValueTask ITransactionStateMachine.RemoveChild(ITransactionStateMachine child, CancellationToken cancellationToken) => StateMachine.RemoveChild(child, cancellationToken);
    ValueTask ITransactionStateMachine.SetParent(ITransactionStateMachine parent, CancellationToken cancellationToken) => StateMachine.SetParent(parent, cancellationToken);
    ValueTask ITransactionStateMachine.RemoveParent(CancellationToken cancellationToken) => StateMachine.RemoveParent(cancellationToken);

    Task ITransactionStateMachine.Rollback(CancellationToken cancellationToken) => StateMachine.Rollback(cancellationToken);
    Task ITransactionStateMachine.Commit(CancellationToken cancellationToken) => StateMachine.Commit(cancellationToken);

    ValueTask<Boolean> ITransactionStateMachine.GetIsCommit(CancellationToken cancellationToken) => StateMachine.GetIsCommit(cancellationToken);
    ValueTask ITransactionStateMachine.RequestFlush(CancellationToken cancellationToken) => StateMachine.RequestFlush(cancellationToken);

    Task ITransactionStateMachine.Flush(Boolean forceRollback, CancellationToken cancellationToken) => StateMachine.Flush(forceRollback, cancellationToken);
    #endregion
}