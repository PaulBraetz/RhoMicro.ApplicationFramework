namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Abstract factory for producing instances of <see cref="IDynamicMultiControlItemModel{TSubControlModel}"/>.
/// </summary>
/// <typeparam name="TSubControlModel">The type of subcontrol encapsulated.</typeparam>
public interface IDynamicMultiControlItemModelFactory<TSubControlModel> :
    IFactory<IDynamicMultiControlItemModel<TSubControlModel>>
{ }
