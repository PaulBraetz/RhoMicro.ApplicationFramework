namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Implementation of <see cref="IFactory{T}"/> that produces instances of <see cref="ButtonModel"/>.
/// </summary>
public sealed class ButtonModelFactory : IFactory<IButtonModel>
{
    /// <inheritdoc/>
    public IButtonModel Create() => new ButtonModel();
}
