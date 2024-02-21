namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Implementation of <see cref="IFactory{T}"/> that produces instances of <see cref="DynamicMultiControlItemModel{TSubControlModel}"/>.
/// </summary>
/// <typeparam name="TSubControlModel">The type of subcontrol model newly created items should have.</typeparam>
public sealed class DynamicMultiControlItemModelFactory<TSubControlModel> : IDynamicMultiControlItemModelFactory<TSubControlModel>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="buttonModelFactory">The factory to invoke when creating a new remove button for new multicontrol item instances.</param>
    /// <param name="subControlModelFactory">The factory to invoke when creating a subcontrol model for new multicontrol item instances.</param>
    public DynamicMultiControlItemModelFactory(IFactory<IButtonModel> buttonModelFactory, IFactory<TSubControlModel> subControlModelFactory)
    {
        _buttonModelFactory = buttonModelFactory;
        _subControlModelFactory = subControlModelFactory;
    }
    private readonly IFactory<IButtonModel> _buttonModelFactory;
    private readonly IFactory<TSubControlModel> _subControlModelFactory;
    /// <inheritdoc/>
    public IDynamicMultiControlItemModel<TSubControlModel> Create()
    {
        var subControlModel = _subControlModelFactory.Create();
        var removeButton = _buttonModelFactory.Create();
        var result = new DynamicMultiControlItemModel<TSubControlModel>(subControlModel, removeButton);

        return result;
    }
}
