namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Exceptions;

/// <summary>
/// Default implementation of <see cref="IButtonModel"/>.
/// </summary>
public sealed class ButtonModel : HasObservableProperties, IButtonModel
{
    private Int32 _loading;
    private Boolean _disabled;
    private Boolean _allowParallelClicks;
    private String? _label;

    /// <inheritdoc/>
    public event AsyncEventHandler? Clicked;

    /// <inheritdoc/>
    public String? Label
    {
        get => _label;
        set => ExchangeBackingField(ref _label, value);

    }
    /// <inheritdoc/>
    public Boolean Disabled
    {
        get => _disabled;
        set => ExchangeBackingField(ref _disabled, value);
    }
    /// <inheritdoc/>
    public Boolean Loading => BooleanState.ToBooleanState(_loading);
    /// <inheritdoc/>
    public Boolean AllowParallelClicks
    {
        get => _allowParallelClicks;
        set => ExchangeBackingField(ref _allowParallelClicks, value);
    }

    /// <inheritdoc/>
    public Task Click(CancellationToken cancellationToken)
    {
        if(Disabled)
            throw StaticExceptions.InvalidOperation.ButtonDisabled;

        var result = !ExchangeLoading(true) || AllowParallelClicks ?
            GetContinuedClick(cancellationToken) :
            Task.CompletedTask;

        return result;
    }
    private async Task GetContinuedClick(CancellationToken cancellationToken)
    {
        try
        {
            await Clicked.InvokeAsync(this, cancellationToken).ConfigureAwait(continueOnCapturedContext: true);
        } finally
        {
            _ = ExchangeLoading(false);
        }
    }

    /// <summary>
    /// Atomically exchanges the value of <see cref="Loading"/> with the parameter supplied.
    /// </summary>
    /// <param name="loading">The value to write to <see cref="Loading"/>.</param>
    /// <returns>The value previously stored in <see cref="Loading"/>.</returns>
    private Boolean ExchangeLoading(Boolean loading)
    {
        var newState = BooleanState.FromBooleanState(loading);
        var oldState = Interlocked.Exchange(ref _loading, newState);

        if(oldState != newState)
        {
            InvokePropertyValueChanged(
                nameof(Loading),
                newValue: loading,
                oldValue: !loading);
        }

        return BooleanState.ToBooleanState(oldState);
    }
}
