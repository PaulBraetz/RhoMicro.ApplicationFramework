namespace RhoMicro.ApplicationFramework.Common.Results;
/// <summary>
/// Represents a cancelled operations result if the cancellation occurred before any 
/// transactional operations or the operation was otherwise able to compliantly exit
/// prematurely in response to its cancellation being requested via a 
/// <see cref="CancellationToken"/> or other mechanisms.
/// </summary>
public sealed class CompliantlyCancelledResult : AwaitableResultBase<CompliantlyCancelledResult>
{
    private CompliantlyCancelledResult() { }
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static CompliantlyCancelledResult Instance { get; } = new();
    /// <inheritdoc/>
    public override String ToString() => nameof(CompliantlyCancelledResult);
}
