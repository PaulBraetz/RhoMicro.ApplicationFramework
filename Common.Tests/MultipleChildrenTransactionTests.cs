#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.ApplicationFramework.Common.Tests;

using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

[TestClass]
public class MultipleChildrenTransactionTests
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ObservableTransactionStateMachine Root { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    [TestInitialize]
    public void Setup() => Root = new ObservableTransactionStateMachine(Comparer);

    [TestMethod]
    public async Task RootFlushRequestFlushesChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.AreEqual(TransactionState.Flushed, child1.State);
        Assert.AreEqual(TransactionState.Flushed, child2.State);
        Assert.AreEqual(TransactionState.Flushed, child3.State);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequesteFlushesToRollbackOnPartiallyUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child2.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await child2.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnPartiallyRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child2.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnPartiallyCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnPartiallyUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child2.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnRolledBackChildren()
    {
        //Arrange
        var root = new ObservableTransactionStateMachine(Comparer);
        var child = new ObservableTransactionStateMachine(Comparer);
        await root.AddChild(child).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child2.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnPartiallyCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedBackRootFlushRequestFlushesToRollbackOnUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedBackRootFlushRequestFlushesToRollbackOnPartiallyUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child2.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await child2.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnPartiallyRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await child2.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestFlushesToCommitOnCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child2.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsTrue(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnPartiallyCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child2).ConfigureAwait(continueOnCapturedContext: false);
        await Root.AddChild(child3).ConfigureAwait(continueOnCapturedContext: false);

        //Act
        await child1.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await child3.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
}
