namespace RhoMicro.ApplicationFramework.Common.Tests;
using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

[TestClass]
public class EventTests
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ObservableTransactionStateMachine Root { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    [TestInitialize]
    public void Setup() => Root = new ObservableTransactionStateMachine(Comparer);

    #region Uncommitted
    [TestMethod]
    public async Task UncommittedRootDoesNotInvokeCommittedOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.IsFalse(fixture.CommittedCalled);
    }
    [TestMethod]
    public async Task UncommittedRootInvokesRolledBackOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.IsTrue(fixture.RolledBackCalled);
    }
    #endregion
    #region RolledBack
    [TestMethod]
    public async Task RolledBackRootDoesNotInvokeCommittedOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.IsFalse(fixture.CommittedCalled);
    }
    [TestMethod]
    public async Task RolledBackRootInvokesRolledBackOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Rollback().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.IsTrue(fixture.RolledBackCalled);
    }
    #endregion
    #region Committed
    [TestMethod]
    public async Task CommittedRootInvokesCommittedOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.IsTrue(fixture.CommittedCalled);
    }
    [TestMethod]
    public async Task CommittedRootDoesNotInvokeRolledBackOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Commit().ConfigureAwait(continueOnCapturedContext: false);
        await Root.RequestFlush().ConfigureAwait(continueOnCapturedContext: false);

        //Assert
        Assert.IsFalse(fixture.RolledBackCalled);
    }
    #endregion
}
