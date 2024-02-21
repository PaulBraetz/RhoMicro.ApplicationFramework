namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using Timer = System.Timers.Timer;

/// <summary>
/// Default implementation of <see cref="IToastModel"/>.
/// </summary>
public sealed class ToastModel : IToastModel, IDisposable
{
    private ToastModel(Timer expiryTimer, String header, String body, ToastType type, TimeSpan lifespan)
    {
        _expiryTimer = expiryTimer;
        Header = header;
        Body = body;
        Type = type;
        Lifespan = lifespan;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    private readonly Timer _expiryTimer;

    /// <inheritdoc/>
    public event EventHandler? HasExpired;

    private const Int32 _stateExpired = 1;
    private const Int32 _stateAlive = 0;

    private Int32 _expiryState = _stateAlive;

    /// <inheritdoc/>
    public Boolean Expired => _expiryState == _stateExpired;
    /// <inheritdoc/>
    public String Header { get; } = String.Empty;
    /// <inheritdoc/>
    public String Body { get; } = String.Empty;
    /// <inheritdoc/>
    public ToastType Type { get; }
    /// <inheritdoc/>
    public TimeSpan Lifespan { get; }
    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Creates and initializes a new instance.
    /// </summary>
    /// <param name="header">The header of the toast.</param>
    /// <param name="body">The body of the toast.</param>
    /// <param name="type">The type of the toast.</param>
    /// <param name="lifespan">The lifespan of the toast</param>
    /// <returns>A new toast instance.</returns>
    public static ToastModel Create(String header, String body, ToastType type, TimeSpan lifespan)
    {
        var timer = new Timer(lifespan.TotalMilliseconds)
        {
            AutoReset = false
        };
        var result = new ToastModel(timer, header, body, type, lifespan);
        timer.Elapsed += (s, e) => result.Expire();
        timer.Start();

        return result;
    }

    /// <inheritdoc/>
    public void Expire()
    {
        if(Interlocked.CompareExchange(ref _expiryState, _stateExpired, _stateAlive) == _stateAlive)
        {
            _expiryTimer.Stop();
            HasExpired?.Invoke(this, EventArgs.Empty);
        }
    }
    /// <inheritdoc/>
    public void Dispose() => _expiryTimer.Dispose();
}