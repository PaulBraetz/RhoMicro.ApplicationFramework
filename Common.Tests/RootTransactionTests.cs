#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
namespace RhoMicro.ApplicationFramework.Common.Tests;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

public class RootTransactionTests
{
    public RootTransactionTests() => Root = new ObservableTransactionStateMachine(Comparer);

    private ObservableTransactionStateMachine Root { get; set; }
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    [Fact]
    public async Task DefaultIsCommitIsFalse()
    {
        //Arrange

        //Act

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task FlushRequestedRootThrowsOnCommit()
    {
        //Arrange

        //Act
        await Root.RequestFlush();

        //Assert
        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () => await Root.Commit());
    }
    [Fact]
    public async Task FlushRequestedRootThrowsOnRollback()
    {
        //Arrange

        //Act
        await Root.RequestFlush();

        //Assert
        _ = await Assert.ThrowsAsync<InvalidOperationException>(async () => await Root.Rollback());
    }
    [Fact]
    public async Task FlushRequestedRootDoesNotThrowOnFlushRequest()
    {
        //Arrange

        //Act
        await Root.RequestFlush();

        //Assert
        await Root.RequestFlush();
    }
    [Fact]
    public async Task RolledBackRootFlushRequestsToFlushed()
    {
        //Arrange

        //Act
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        Assert.Equal(TransactionState.Flushed, Root.State);
    }
    [Fact]
    public async Task RolledBackRootFlushRequestsToRollback()
    {
        //Arrange

        //Act
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task CommittedRootFlushRequestsToFlushed()
    {
        //Arrange

        //Act
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        Assert.Equal(TransactionState.Flushed, Root.State);
    }
    [Fact]
    public async Task CommittedRootFlushRequestsToCommitted()
    {
        //Arrange

        //Act
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.True(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestsToRollback()
    {
        //Arrange

        //Act
        await Root.RequestFlush();

        //Assert
        var condition = await Root.GetIsCommit();
        Assert.False(condition);
    }
    [Fact]
    public async Task UncommittedRootFlushRequestsToFlushed()
    {
        //Arrange

        //Act
        await Root.RequestFlush();

        //Assert
        Assert.Equal(TransactionState.Flushed, Root.State);
    }
}