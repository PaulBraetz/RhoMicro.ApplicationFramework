namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Strategy for conditionally disposing of disposable objects.
/// </summary>
/// <typeparam name="T">The type of objects to conditionally dispose of.</typeparam>
public interface IDisposeStrategy<in T>
{
    /// <summary>
    /// Disposes an instance of <typeparamref name="T"/>, if the policy applies to that Object.
    /// </summary>
    /// <param name="disposable">The Object to conditionally dispose.</param>
    void Dispose(T disposable);
}
