namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Bridge between <see cref="IToastModel"/> and <see cref="Common.Abstractions.IObserver{T}"/>.
/// Values published will be raised as info toasts.
/// </summary>
/// <typeparam name="T">The type of command to execute.</typeparam>
public sealed class ToastsSuccessModel<T> : Common.Abstractions.IObserver<T>, IToastsModel
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="toastModelFactory">The factory to use when creating new toast models for exceptions received.</param>
    /// <param name="toastComparer">The comparer to use when relating a toasts <see cref="IToastModel.HasExpired"/> event to the toast to remove.</param>
    /// <param name="valueFormatter">The formatter used to create the toasts body upon notification of a new value.</param>
    public ToastsSuccessModel(
        IToastModelFactory toastModelFactory,
        IEqualityComparer<IToastModel> toastComparer,
        IStaticFormatter<T> valueFormatter)
    {
        _toastModelFactory = toastModelFactory;
        _valueFormatter = valueFormatter;
        _toasts = new(toastComparer);
    }

    private const Int32 _charDisplayMillis = 50;
    private const Int32 _minDisplayMillis = 5_000;
    private const String _toastHeader = "Info";

    private readonly HashSet<IToastModel> _toasts;
    private readonly IStaticFormatter<T> _valueFormatter;
    private readonly IToastModelFactory _toastModelFactory;

    /// <inheritdoc/>
    public void Notify(T value)
    {
        var body = _valueFormatter.Format(value);
        var minDisplayMillis = body.Length * _charDisplayMillis;
        var displayMillis = Math.Max(minDisplayMillis, _minDisplayMillis);
        var lifespan = TimeSpan.FromMilliseconds(displayMillis);
        var toast = _toastModelFactory.Create(_toastHeader, body, ToastType.Info, lifespan);
        AddToast(toast);
    }

    private void AddToast(IToastModel toast)
    {

        var added = _toasts.Add(toast);
        if(added)
        {
            InvokeToastsChanged(ToastsChangedType.Added, toast);
            toast.HasExpired += (s, e) => RemoveToast(toast);
        }
    }
    private void RemoveToast(IToastModel toast)
    {
        var removed = _toasts.Remove(toast);
        if(removed)
        {
            InvokeToastsChanged(ToastsChangedType.Removed, toast);
        }
    }

    private void InvokeToastsChanged(ToastsChangedType type, IToastModel toastAffected) =>
        ToastsChanged?.Invoke(this, new ToastChangedEventArgs(type, toastAffected));

    /// <inheritdoc/>
    public ICollection<IToastModel> GetToasts()
    {
        var toastModels = _toasts.ToList();

        return toastModels;
    }

    /// <inheritdoc/>
    public event EventHandler<ToastChangedEventArgs>? ToastsChanged;
}
