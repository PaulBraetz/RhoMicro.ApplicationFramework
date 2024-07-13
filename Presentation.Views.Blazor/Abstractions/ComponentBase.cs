namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Components;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Hosting;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Exceptions;
using RhoMicro.RequiredPropertyValidation;

/// <summary>
/// <inheritdoc/>
/// </summary>
public abstract class ComponentBase : ComponentBase<ICssStyle>, IComponent;

/// <summary>
/// Base component providing non captured parameters via <see cref="Attributes"/>
/// as well as providing automated checking for required parameters in <see cref="OnParametersSet"/>.
/// Required parameters must be annotated using the <see cref="ParameterAttribute"/> and the <see cref="RequiredAttribute"/>.
/// Upon encountering a required parameter with value <see langword="null"/>,
/// a <see cref="NullComponentParameterException"/> will be thrown.
/// </summary>
/// <typeparam name="TStyle">The type of style received by this component.</typeparam>
#pragma warning disable CA1063 // Implement IDisposable Correctly
public abstract class ComponentBase<TStyle> : SimpleInjectorIntegratedComponent, IDisposable, IComponent<TStyle>
    where TStyle : ICssStyle
#pragma warning restore CA1063 // Implement IDisposable Correctly
{
    private readonly CancellationTokenSource _disposalCts = new();
    private Int32 _disposed = BooleanState.FalseState;
    private Dictionary<String, Object?>? _attributes;
#pragma warning disable CS8618 // required via Style prop
    private TStyle _style;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <inheritdoc/>
    [Injected]
    [Parameter]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Setting ClassNames appropriately is required")]
    public required TStyle Style
    {
        get => _style;
        set
        {
            _style = value;
            ClassNames = ClassNames.Add(Style.ClassNames);
        }
    }

    /// <inheritdoc/>
    [Parameter(CaptureUnmatchedValues = true)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Impossible due to EnsureComponentType")]
    public required Dictionary<String, Object?> Attributes
    {
        get
        {
            if(_attributes == null)
            {
                _attributes = [];
                EnsureComponentTypeAttributes();
            }

            return _attributes;
        }
        set
        {
            _attributes = value;
            EnsureComponentTypeAttributes();
        }
    }
    /// <summary>
    /// Gets or sets the validator used to validate required non-null properties of component instances.
    /// </summary>
    [Injected]
    public required RequiredPropertyValidator RequiredPropertyValidator { get; set; }
    /// <summary>
    /// Gets a representation of the value associated to the <c>class</c> attribute or an empty set if no value is associated.
    /// </summary>
    protected CssClassNames ClassNames
    {
        get => Attributes.TryGetValue("class", out var classNames) ? CssClassNames.Create(classNames) : CssClassNames.Empty;
        set => Attributes["class"] = value;
    }
    /// <summary>
    /// Gets a cancellation token that will be cancelled upon <see cref="Dispose"/> being called.
    /// </summary>
    protected CancellationToken ComponentDisposalToken => _disposalCts.Token;

    private void EnsureComponentTypeAttributes()
    {
        if(ExecutionEnvironment is null || !ExecutionEnvironment.Configuration.IsDevelopment())
            return;

        Attributes["development-component-type"] = ComponentType.FullName;
    }

    /// <inheritdoc/>
    protected override void OnParametersSet() => RequiredPropertyValidator.Validate(this);

    /// <summary>
    /// Called the first time the instance is disposed.
    /// </summary>
    protected virtual void OnDisposing() { }

    /// <inheritdoc/>
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        if(Interlocked.Exchange(
            ref _disposed,
            BooleanState.TrueState) == BooleanState.FalseState)
        {
            _disposalCts.Cancel();
            _disposalCts.Dispose();
            OnDisposing();
        }
    }
}
