namespace RhoMicro.ApplicationFramework.Presentation.Models;
using System.Timers;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using Timer = System.Timers.Timer;

/// <summary>
/// Default implementation of <see cref="IAgeModel"/>.
/// </summary>
public sealed class AgeModel : HasObservableProperties, IAgeModel, IDisposable
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public AgeModel()
    {
        FormatAge = DefaultFormat;
        _updateTimer.Elapsed += Update;
    }

    /// <inheritdoc/>
    public Func<TimeSpan, String> FormatAge { get; set; }
    /// <inheritdoc/>
    public DateTimeOffset Origin
    {
        get => _origin;
        set
        {
            lock(_syncRoot)
            {
                _updateTimer.Stop();
                _origin = value;
                _updateTimer.Interval = 1000;
                _updateTimer.Start();
            }
        }
    }
    /// <inheritdoc/>
    public String Value
    {
        get => _value;
        private set => ExchangeBackingField(ref _value, value);
    }

    private String _value = String.Empty;
    private DateTimeOffset _origin;
    private readonly Timer _updateTimer = new()
    {
        AutoReset = true
    };
    private readonly Object _syncRoot = new();

    private void Update(Object? sender, ElapsedEventArgs eventArgs)
    {
        var now = DateTimeOffset.Now;
        var age = now - _origin;
        var ageFormatted = FormatAge.Invoke(age);
        Value = ageFormatted;
    }

    private String DefaultFormat(TimeSpan age)
    {
        String result;
        if(age < TimeSpan.FromMinutes(1))
        {
            result = $"{age.Seconds} secs ago";
        } else if(age < TimeSpan.FromHours(1))
        {
            result = $"{age.Minutes} mins ago";
            _updateTimer.Interval = 60_000;
        } else if(age < TimeSpan.FromDays(1))
        {
            result = $"{age.Hours} hrs ago";
            _updateTimer.Interval = 3_600_000;
        } else
        {
            result = $"{age.Days} days ago";
        }

        return result;
    }
    /// <inheritdoc/>
    public void Dispose() => _updateTimer.Dispose();
}
