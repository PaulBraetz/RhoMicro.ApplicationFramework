namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Intercepts arbitrary objects.
/// </summary>
/// <typeparam name="T">
/// The type of object to intercept.
/// </typeparam>
public interface IInterceptor<T>
{
    /// <summary>
    /// Intercepts an object.
    /// </summary>
    /// <param name="obj">The object to intercept.</param>
    /// <param name="cancellationToken">
    /// The cancellation token used to interception to be cancelled.
    /// </param>
    /// <returns>
    /// A task that, upon completion, will contain the (potentially) transformed object resulting from the
    /// interception.
    /// </returns>
    ValueTask<T> Intercept(T obj, CancellationToken cancellationToken);
}
