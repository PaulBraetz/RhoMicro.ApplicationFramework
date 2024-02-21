namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Bridge between <see cref="IToastModel"/> and <see cref="Common.Abstractions.IObserver{T}"/>.
/// Exceptions observed will be raised as error toasts.
/// </summary>
public sealed class ToastsErrorModel : Common.Abstractions.IObserver<Exception>, IToastsModel
{
    /// <summary> 
    /// Initializes a new instance.
    /// </summary>
    /// <param name="toastModelFactory">The factory to use when creating new toast models for exceptions received.</param>
    /// <param name="toastComparer">The comparer to use when relating a toasts <see cref="IToastModel.HasExpired"/> event to the toast to remove.</param>
    public ToastsErrorModel(IToastModelFactory toastModelFactory, IEqualityComparer<IToastModel> toastComparer)
    {
        _toastModelFactory = toastModelFactory;
        _toasts = new HashSet<IToastModel>(toastComparer);
    }

    private const Int32 _charDisplayMillis = 50;
    private const Int32 _minDisplayMillis = 5_000;
    private const String _toastHeader = "Error";

    private readonly HashSet<IToastModel> _toasts;

    private readonly IToastModelFactory _toastModelFactory;

    /// <inheritdoc/>
    public void Notify(Exception value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var minDisplayMillis = value.Message.Length * _charDisplayMillis;
        var displayMillis = Math.Max(minDisplayMillis, _minDisplayMillis);
        var lifespan = TimeSpan.FromMilliseconds(displayMillis);
        var toast = _toastModelFactory.Create(_toastHeader, value.Message, ToastType.Error, lifespan);
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
