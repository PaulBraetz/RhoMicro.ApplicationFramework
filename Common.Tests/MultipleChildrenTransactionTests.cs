#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.ApplicationFramework.Common.Tests;

using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

public class MultipleChildrenTransactionTests
{
    public MultipleChildrenTransactionTests() => Root = new ObservableTransactionStateMachine(Comparer);

    private ObservableTransactionStateMachine Root { get; set; }
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    [Fact]
    public async Task RootFlushRequestFlushesChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await Root.RequestFlush();

        //Assert
        Assert.Equal(TransactionState.Flushed, child1.State);
        Assert.Equal(TransactionState.Flushed, child2.State);
        Assert.Equal(TransactionState.Flushed, child3.State);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequesteFlushesToRollbackOnPartiallyUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child2.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Rollback();
        await child2.Rollback();
        await child3.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnPartiallyRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Rollback();
        await child3.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Commit();
        await child2.Commit();
        await child3.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnPartiallyCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Commit();
        await child3.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnPartiallyUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child2.Commit();
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnRolledBackChildren()
    {
        //Arrange
        var root = new ObservableTransactionStateMachine(Comparer);
        var child = new ObservableTransactionStateMachine(Comparer);
        await root.AddChild(child);

        //Act
        await child.Rollback();
        await root.Rollback();
        await root.RequestFlush();

        //Assert
        var condition = await root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Commit();
        await child2.Commit();
        await child3.Commit();
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnPartiallyCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Commit();
        await child3.Commit();
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedBackRootFlushRequestFlushesToRollbackOnUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedBackRootFlushRequestFlushesToRollbackOnPartiallyUncommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child2.Commit();
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Rollback();
        await child2.Rollback();
        await child3.Rollback();
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnPartiallyRolledBackChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Rollback();
        await child2.Commit();
        await child3.Rollback();
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestFlushesToCommitOnCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Commit();
        await child2.Commit();
        await child3.Commit();
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.True(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnPartiallyCommittedChildren()
    {
        //Arrange
        var child1 = new ObservableTransactionStateMachine(Comparer);
        var child2 = new ObservableTransactionStateMachine(Comparer);
        var child3 = new ObservableTransactionStateMachine(Comparer);

        await Root.AddChild(child1);
        await Root.AddChild(child2);
        await Root.AddChild(child3);

        //Act
        await child1.Commit();
        await child3.Commit();
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
}
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
