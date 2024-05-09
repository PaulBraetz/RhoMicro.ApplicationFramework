namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IAuthenticationConsumerModel"/> and <see cref="IAuthenticationProviderModel"/>.
/// </summary>
public sealed class AuthenticationModel : HasObservableProperties, IAuthenticationConsumerModel, IAuthenticationProviderModel
{
    private Int32 _isAuthenticated;

    /// <inheritdoc/>
    public Boolean IsAuthenticated
    {
        get => BooleanState.ToBooleanState(_isAuthenticated);
        set => base.ExchangeBackingField(ref _isAuthenticated, BooleanState.FromBooleanState(value));
    }
}
