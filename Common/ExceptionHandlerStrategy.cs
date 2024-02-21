namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Strategy-based implementation of <see cref="IExceptionHandler"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="canHandle">The strategy to invoke when calling <see cref="CanHandle(Exception)"/>.</param>
/// <param name="handle">The strategy to invoke when calling <see cref="Handle"/>.</param>
public sealed class ExceptionHandlerStrategy(Func<Exception, Boolean> canHandle, Func<Exception, ValueTask> handle) : IExceptionHandler
{
    /// <inheritdoc/>
    public Boolean CanHandle(Exception ex) => canHandle(ex);
    /// <inheritdoc/>
    public ValueTask Handle(Exception ex) =>
        CanHandle(ex) ?
        handle(ex) :
        throw new InvalidOperationException($"Unable to handle exception {ex}", ex);
}
