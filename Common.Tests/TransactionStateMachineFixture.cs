namespace RhoMicro.ApplicationFramework.Common.Tests;

using RhoMicro.ApplicationFramework.Common.Transactions;

internal sealed class TransactionStateMachineFixture
{
    public TransactionStateMachineFixture(ObservableTransactionStateMachine sut)
    {
        _sut = sut;
        _sut.Committed += (s, e) =>
        {
            CommittedCalled = true;
            return Task.CompletedTask;
        };
        _sut.RolledBack += (s, e) =>
        {
            RolledBackCalled = true;
            return Task.CompletedTask;
        };
    }

    private readonly ObservableTransactionStateMachine _sut;

    public Boolean CommittedCalled { get; private set; }
    public Boolean RolledBackCalled { get; private set; }
}
