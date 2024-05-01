#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace RhoMicro.ApplicationFramework.Common.Tests;
using RhoMicro.ApplicationFramework.Common.Transactions;
using RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

public class EventTests
{
    public EventTests() => Root = new ObservableTransactionStateMachine(Comparer);

    private ObservableTransactionStateMachine Root { get; set; }
    private IEqualityComparer<ITransactionStateMachine> Comparer { get; } = TransactionStateMachineEqualityComparer.Instance;

    #region Uncommitted
    [Fact]
    public async Task UncommittedRootDoesNotInvokeCommittedOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.RequestFlush();

        //Assert
        Assert.False(fixture.CommittedCalled);
    }
    [Fact]
    public async Task UncommittedRootInvokesRolledBackOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.RequestFlush();

        //Assert
        Assert.True(fixture.RolledBackCalled);
    }
    #endregion
    #region RolledBack
    [Fact]
    public async Task RolledBackRootDoesNotInvokeCommittedOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        Assert.False(fixture.CommittedCalled);
    }
    [Fact]
    public async Task RolledBackRootInvokesRolledBackOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Rollback();
        await Root.RequestFlush();

        //Assert
        Assert.True(fixture.RolledBackCalled);
    }
    #endregion
    #region Committed
    [Fact]
    public async Task CommittedRootInvokesCommittedOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        Assert.True(fixture.CommittedCalled);
    }
    [Fact]
    public async Task CommittedRootDoesNotInvokeRolledBackOnFlush()
    {
        //Arrange
        var fixture = new TransactionStateMachineFixture(Root);

        //Act
        await Root.Commit();
        await Root.RequestFlush();

        //Assert
        Assert.False(fixture.RolledBackCalled);
    }
    #endregion
}
