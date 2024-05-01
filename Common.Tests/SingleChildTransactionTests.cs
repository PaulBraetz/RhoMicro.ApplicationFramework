#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.ApplicationFramework.Common.Tests;

using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

public class SingleChildTransactionTests
{
    public SingleChildTransactionTests() => Root = new ObservableTransactionStateMachine(Comparer);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ObservableTransactionStateMachine Root { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    [Fact]
    public async Task AddChildThrowsOnFlushedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);

        //Act
        await child.RequestFlush();

        //Assert
        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () => await Root.AddChild(child));
    }
    [Fact]
    public async Task SetParentThrowsOnFlushedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);

        //Act
        await child.RequestFlush();

        //Assert
        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () => await child.SetParent(Root));
    }
    [Fact]
    public async Task ChildFlushRequestFlushesToImmutable()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await child.RequestFlush();

        //Assert
        Assert.Equal(TransactionState.Immutable, child.State);
    }
    [Fact]
    public async Task RootFlushRequestFlushesChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await Root.RequestFlush();

        //Assert
        Assert.Equal(TransactionState.Flushed, child.State);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnUncommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnRolledBackChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await child.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestFlushesToRollbackOnCommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await child.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnUncommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnRolledBackChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await child.Rollback();
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestFlushesToRollbackOnCommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await child.Commit();
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnUncommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestFlushesToRollbackOnRolledBackChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await child.Rollback();
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestFlushesToCommitOnCommittedChild()
    {
        //Arrange
        var child = new ObservableTransactionStateMachine(Comparer);
        await Root.AddChild(child);

        //Act
        await child.Commit();
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.True(condition);
    }
}