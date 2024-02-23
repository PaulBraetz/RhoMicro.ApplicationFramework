namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Composite of <see cref="IToastsModel"/> children.
/// </summary>
public sealed class ToastsCompositeModel : IToastsModel
{
    private ToastsCompositeModel(IEnumerable<IToastsModel> children) => _children = children;
    private readonly IEnumerable<IToastsModel> _children;
    /// <inheritdoc/>
    public event EventHandler<ToastChangedEventArgs>? ToastsChanged;
    /// <summary>
    /// Creates and initializes a new instance.
    /// </summary>
    /// <param name="children">The composites children.</param>
    /// <returns>A new composite toasts model.</returns>
    public static ToastsCompositeModel Create(IEnumerable<IToastsModel> children)
    {
        ArgumentNullException.ThrowIfNull(children);

        var result = new ToastsCompositeModel(children);
        foreach(var child in children)
        {
            child.ToastsChanged += result.OnToastsChanged;
        }

        return result;
    }
    /// <inheritdoc/>
    public ICollection<IToastModel> GetToasts()
    {
        var result = _children.SelectMany(c => c.GetToasts()).ToList();

        return result;
    }
    private void OnToastsChanged(Object? sender, ToastChangedEventArgs eventArgs) =>
        InvokeToastsChanged(eventArgs.Type, eventArgs.ToastAffected);
    private void InvokeToastsChanged(ToastsChangedType type, IToastModel toastAffected) =>
        ToastsChanged?.Invoke(this, new ToastChangedEventArgs(type, toastAffected));
}
