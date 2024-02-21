namespace RhoMicro.ApplicationFramework.Common.Tests;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

[TestClass]
public class RootTransactionTests
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ObservableTransactionStateMachine Root { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    [TestInitialize]
    public void Setup() => Root = new ObservableTransactionStateMachine(Comparer);

    [TestMethod]
    public async Task DefaultIsCommitIsFalse()
    {
        //Arrange

        //Act

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task FlushRequestedRootThrowsOnCommit()
    {
        //Arrange

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        _ = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await Root.Commit().ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
    }
    [TestMethod]
    public async Task FlushRequestedRootThrowsOnRollback()
    {
        //Arrange

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        _ = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
    }
    [TestMethod]
    public async Task FlushRequestedRootDoesNotThrowOnFlushRequest()
    {
        //Arrange

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestsToFlushed()
    {
        //Arrange

        //Act
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.AreEqual(TransactionState.Flushed, Root.State);
    }
    [TestMethod]
    public async Task RolledBackRootFlushRequestsToRollback()
    {
        //Arrange

        //Act
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestsToFlushed()
    {
        //Arrange

        //Act
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.AreEqual(TransactionState.Flushed, Root.State);
    }
    [TestMethod]
    public async Task CommittedRootFlushRequestsToCommitted()
    {
        //Arrange

        //Act
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsTrue(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestsToRollback()
    {
        //Arrange

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        var condition = await Root.GetIsCommit().ConfigureAwait(continueOnCapturedContext: false);
        Assert.IsFalse(condition);
    }
    [TestMethod]
    public async Task UncommittedRootFlushRequestsToFlushed()
    {
        //Arrange

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.AreEqual(TransactionState.Flushed, Root.State);
    }
}