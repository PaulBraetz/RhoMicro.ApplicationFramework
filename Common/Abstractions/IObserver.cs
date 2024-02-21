namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Observes value publishers.
/// </summary>
/// <typeparam name="T">The type of value observed.</typeparam>
public interface IObserver<in T>
{
    /// <summary>
    /// Notifies the observer about a new value having been published.
    /// </summary>
    /// <param name="value">The value published.</param>
    void Notify(T value);
}
