namespace RhoMicro.ApplicationFramework.Aspects;

/// <summary>
/// Represents a subscription to the ambient <see cref="Progress"/>.
/// </summary>
public readonly struct ProgressSubscription<T> : IDisposable, IEquatable<ProgressSubscription<T>>
{
    internal ProgressSubscription(Guid id, AggregateProgress<T> aggregate, IProgress<T> progress)
    {
        _id = id;
        _aggregate = aggregate;

        Progress = progress;
    }

    private readonly Guid _id;
    private readonly AggregateProgress<T> _aggregate;

    /// <summary>
    /// Gets the progress whose ambient registration is represented by this instance.
    /// </summary>
    public IProgress<T> Progress { get; }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public void Dispose() => _aggregate?.Remove(_id);
    public override Boolean Equals(Object? obj) => obj is ProgressSubscription<T> subscription && Equals(subscription);
    public Boolean Equals(ProgressSubscription<T> other) => _id.Equals(other._id);
    public override Int32 GetHashCode() => HashCode.Combine(_id);
    public static Boolean operator ==(ProgressSubscription<T> left, ProgressSubscription<T> right) => left.Equals(right);
    public static Boolean operator !=(ProgressSubscription<T> left, ProgressSubscription<T> right) => !( left == right );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
