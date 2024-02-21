#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.ApplicationFramework.Common.Tests;

using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

[TestClass]
public class SingleChildTransactionTests
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ObservableTransactionStateMachine Root { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    [TestInitialize]
    public void Setup() => Root = new ObservableTransactionStateMachine(Comparer);

    [TestMethod]
    public async Task AddChildThrowsOnFlushedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);

        //Act
        await child.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        _ = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
    }
    [TestMethod]
    public async Task SetParentThrowsOnFlushedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);

        //Act
        await child.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        _ = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await child.SetParent(Root).ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
    }
    [TestMethod]
    public async Task ChildFlushRequestFlushesToImmutable()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.AreEqual(TransactionState.Immutable, child.State);
    }
    [TestMethod]
    public async Task RootFlushRequestFlushesChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.AreEqual(TransactionState.Flushed, child.State);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnUncommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnRolledBackChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnCommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnUncommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnRolledBackChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnCommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnUncommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnRolledBackChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestFlushesToCommitOnCommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsTrue(condition);
    }
}