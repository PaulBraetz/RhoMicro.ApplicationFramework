namespace RhoMicro.ApplicationFramework.Common.Exceptions;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Transactions;

internal static class Throw
{
    public static class NotSupportedException
    {
        public static System.NotSupportedException ClosureEquals =>
            new($"{typeof(RequestClosure<,>).Name}.Equals is not supported.");
        public static System.NotSupportedException ClosureGetHashCode =>
            new($"{typeof(RequestClosure<,>).Name}.GetHashCode is not supported.");
        public static System.NotSupportedException CallbackDisposableEquals =>
            new($"{typeof(CallbackDisposable).Name}.Equals is not supported.");
        public static System.NotSupportedException CallbackDisposableGetHashCode =>
            new($"{typeof(CallbackDisposable).Name}.GetHashCode is not supported.");
        public static System.NotSupportedException TransactionFlushingDisposableEquals =>
            new($"{typeof(TransactionFlushingDisposable<>).Name}.Equals is not supported.");
        public static System.NotSupportedException TransactionFlushingDisposableGetHashCode =>
            new($"{typeof(TransactionFlushingDisposable<>).Name}.GetHashCode is not supported.");
    }
    public static class ObjectDisposedException
    {
        public static System.ObjectDisposedException Scope =>
            new(nameof(Scope));
        public static System.ObjectDisposedException LateScope =>
            new(nameof(LateScope));
        public static System.ObjectDisposedException LateConditionalScope =>
            new(nameof(LateConditionalScope));
    }
    public static class InvalidOperationException
    {
        public static System.InvalidOperationException ScopeCount =>
            new("The amount of scopes disposed cannot exceed the amount of scopes registered.");
    }
}
