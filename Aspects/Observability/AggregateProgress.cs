namespace RhoMicro.ApplicationFramework.Aspects;

using System.Collections.Concurrent;

using RhoMicro.ApplicationFramework.Common;

internal sealed class AggregateProgress<T> : IProgress<T>
{
    private readonly ConcurrentDictionary<Guid, IProgress<T>> _progresses = new();
    public Guid Add(IProgress<T> progress)
    {
        var id = Guid.NewGuid();
        _progresses[id] = progress;
        return id;
    }
    public void Remove(Guid id) => _progresses.Remove(id, out _);
    public void Report(T value) => _progresses.Values.ForEach(p => p.Report(value));
}