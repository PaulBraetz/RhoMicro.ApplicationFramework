namespace RhoMicro.ApplicationFramework.Common.Transactions.Abstractions;

using System;
using System.Threading.Tasks;

public static class Transactions
{
    sealed class Context
    {
        private Context() { }
        private static readonly AsyncLocal<Context> _local = new();
        private static readonly SemaphoreSlim _instanceGate = new(1, 1);
        public static Context Instance
        {
            get
            {
                if(_local.Value == null)
                {
                    using var _ = _instanceGate.WaitDisposable();
                    _local.Value ??= new Context();
                }

                return _local.Value;
            }
        }

        private readonly SemaphoreSlim _transactionGate = new(1, 1);
        private ITransaction? _transaction;

        public async Task<ITransaction> GetParent<TTransaction>(ITransactionFactory<TTransaction> factory, CancellationToken ct)
            where TTransaction : ITransaction
        {
            if(_transaction is { })
                return _transaction;

            using var _ = await _transactionGate.WaitDisposableAsync(ct);

            if(_transaction is { })
                return _transaction;

            _transaction = await factory.CreateRoot(ct);
            return _transaction;
        }
    }

    public static Task<ITransaction> GetParent<TTransaction>(
        ITransactionFactory<TTransaction> factory,
        CancellationToken ct)
        where TTransaction : ITransaction
    {
        ArgumentNullException.ThrowIfNull(factory);
        var result = Context.Instance.GetParent(factory, ct);
    }
}
