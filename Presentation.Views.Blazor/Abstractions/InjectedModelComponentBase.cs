namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// <inheritdoc />
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
public abstract class InjectedModelComponentBase<TModel> : InjectedModelComponentBase<TModel, ICssStyle>;

/// <summary>
/// Base component view for a model of <typeparamref name="TModel"/>. The model will be injected.
/// </summary>
/// <typeparam name="TModel">The type of model to render a component for.</typeparam>
/// <typeparam name="TStyle">The type of style received by this component.</typeparam>
public abstract class InjectedModelComponentBase<TModel, TStyle> : ModelComponentBase<TModel, TStyle>
    where TStyle : ICssStyle
{
    /// <inheritdoc/>
    [Injected]
    public override TModel Value { get => base.Value; set => base.Value = value; }

    /// <summary>
    /// Gets or sets the strategy used to possibly dispose the component model.
    /// </summary>
    [Injected]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public IDisposeStrategy<TModel> ModelDisposeStrategy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <inheritdoc/>
    protected override void OnDisposing()
    {
        ModelDisposeStrategy.Dispose(Value);
        base.OnDisposing();
    }
}
