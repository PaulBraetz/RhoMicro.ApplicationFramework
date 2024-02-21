namespace RhoMicro.ApplicationFramework.Presentation.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Base implementation of <see cref="IMultiControlModel{TSubControlModel}"/>.
/// </summary>
/// <typeparam name="TSubControlModel">The type of subcontrol model encapsulated.</typeparam>
public class MultiControlModel<TSubControlModel> : HasObservableProperties, IMultiControlModel<TSubControlModel>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public MultiControlModel()
    {
        var items = new ObservableCollection<TSubControlModel>();
        items.CollectionChanged += OnCollectionChanged;
        Items = items;
    }

    private String _label = String.Empty;
    private String _description = String.Empty;

    private void OnCollectionChanged(Object? _, NotifyCollectionChangedEventArgs e) =>
        InvokePropertyValueChanged(nameof(Items), Items, Items);

    /// <inheritdoc/>
    public ICollection<TSubControlModel> Items { get; }
    /// <inheritdoc/>
    public String Label
    {
        get => _label;
        set => ExchangeBackingField(ref _label, value);
    }
    /// <inheritdoc/>
    public String Description
    {
        get => _description;
        set => ExchangeBackingField(ref _description, value);
    }
}
