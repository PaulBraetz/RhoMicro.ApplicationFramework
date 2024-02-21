namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IDynamicMultiControlModel{TSubControlModel}"/>.
/// </summary>
/// <typeparam name="TSubControlModel"></typeparam>
public sealed class DynamicMultiControlModel<TSubControlModel> :
    MultiControlModel<IDynamicMultiControlItemModel<TSubControlModel>>,
    IDynamicMultiControlModel<TSubControlModel>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="add">The button model used for adding an item.</param>
    /// <param name="itemFactory">The factory to invoke when adding a new item to the multicontrol.</param>
    public DynamicMultiControlModel(IButtonModel add, IDynamicMultiControlItemModelFactory<TSubControlModel> itemFactory)
    {
        _itemFactory = itemFactory;

        Add = add;
        Add.Clicked += OnAdd;
        Add.Label = "+";
    }

    private readonly IDynamicMultiControlItemModelFactory<TSubControlModel> _itemFactory;

    /// <inheritdoc/>
    public IButtonModel Add { get; }

    private Task OnAdd(Object? _0, IAsyncEventArguments _1)
    {
        var item = _itemFactory.Create();
        item.Remove.Clicked += (s, e) =>
        {
            _ = Items.Remove(item);
            return Task.CompletedTask;
        };

        Items.Add(item);

        return Task.CompletedTask;
    }
}
