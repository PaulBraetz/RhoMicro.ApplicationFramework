namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Implementation of <see cref="IFactory{T}"/> that produces instances of <see cref="DynamicMultiControlItemModel{TSubControlModel}"/>.
/// </summary>
/// <typeparam name="TSubControlModel">The type of subcontrol model newly created items should have.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="buttonModelFactory">The factory to invoke when creating a new remove button for new multicontrol item instances.</param>
/// <param name="subControlModelFactory">The factory to invoke when creating a subcontrol model for new multicontrol item instances.</param>
public sealed class DynamicMultiControlItemModelFactory<TSubControlModel>(IFactory<IButtonModel> buttonModelFactory, IFactory<TSubControlModel> subControlModelFactory) : IDynamicMultiControlItemModelFactory<TSubControlModel>
{
    /// <inheritdoc/>
    public IDynamicMultiControlItemModel<TSubControlModel> Create()
    {
        var subControlModel = subControlModelFactory.Create();
        var removeButton = buttonModelFactory.Create();
        var result = new DynamicMultiControlItemModel<TSubControlModel>(subControlModel, removeButton);

        return result;
    }
}
