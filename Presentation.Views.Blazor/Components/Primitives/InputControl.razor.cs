namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Components.Primitives;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Reactive.Subjects;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Represents the style applied to a <see cref="InputControl{TValue, TError}"/> component.
/// </summary>
public interface IInputControlCssStyle : ICssStyle
{
    /// <summary>
    /// The additional style to apply when displaying a valid value (<see cref="InputValidityType.Valid"/>).
    /// </summary>
    ICssStyle ValidStyle { get; }
    /// <summary>
    /// The additional style to apply when displaying an invalid value (<see cref="InputValidityType.Invalid"/>).
    /// </summary>
    ICssStyle InvalidStyle { get; }
    /// <summary>
    /// The additional style to apply when displaying a value of unknown validity (<see cref="InputValidityType.None"/>).
    /// </summary>
    ICssStyle NoneValidityStyle { get; }
}

/// <see cref="IInputControlCssStyle"/>
public sealed class InputControlCssStyleSettings : CssStyleSettings, IInputControlCssStyle
{
    /// <inheritdoc cref="IInputControlCssStyle.ValidStyle"/>
    public CssStyleSettings ValidStyle { get; set; } = new();
    ICssStyle IInputControlCssStyle.ValidStyle => ValidStyle;
    /// <inheritdoc cref="IInputControlCssStyle.InvalidStyle"/>
    public CssStyleSettings InvalidStyle { get; set; } = new();
    ICssStyle IInputControlCssStyle.InvalidStyle => InvalidStyle;
    /// <inheritdoc cref="IInputControlCssStyle.NoneValidityStyle"/>
    public CssStyleSettings NoneValidityStyle { get; set; } = new();
    ICssStyle IInputControlCssStyle.NoneValidityStyle => NoneValidityStyle;
}

/// <summary>
/// Generic input control wrapping <c>input</c> elements.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TError"></typeparam>
public partial class InputControl<TValue, TError> : ModelComponentBase<IInputModel<TValue, TError>, IInputControlCssStyle>
{
    /// <summary>
    /// Gets or sets the predicate used to determine if a given input <see cref="String"/> may be parsed to an instance of <typeparamref name="TValue"/>.
    /// </summary>
    [Parameter]
    public Func<String, Boolean>? CanParse { get; set; }
    /// <summary>
    /// Gets or sets the transformation used to parse a given input <see cref="String"/> to an instance of <typeparamref name="TValue"/>.
    /// </summary>
    [Parameter]
    public Func<String, TValue>? Parse { get; set; }
    /// <summary>
    /// Gets or sets the reverse transformation used to calculate the initial input string to display.
    /// </summary>
    [Parameter]
    public required Func<TValue, String> InitialValueParse { get; set; }
    /// <summary>
    /// Gets or sets the transformation used to obtain a display <see cref="String"/> from an instance of <typeparamref name="TError"/>.
    /// </summary>
    [Parameter]
    public Func<TError, String>? ErrorTitleParse { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether new inputs should be parsed upon the <c>oninput</c> (focus lost) or upon registering the <c>onchange</c> (keystroke) event.
    /// </summary>
    [Parameter]
    public Boolean UpdateOnFocusLost { get; set; }

    private String _rawValue = String.Empty;
    private String RawValue
    {
        get => _rawValue;
        set => _rawValueReceiver.OnNext(value);
    }

    private readonly Subject<String> _rawValueReceiver = new();
    private IDisposable? _valueParseSubscription;

    private String _bindEvent = "oninput";

    private Task OnKeyPress(KeyboardEventArgs args)
    {
        if(args.Code is "Enter" or "NumpadEnter")
        {
            return Model.Enter();
        }

        return Task.CompletedTask;
    }
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if(CanParse != null && Parse == null)
        {
            throw new InvalidOperationException($"Unable to parse values: {nameof(Parse)} was not supplied but {nameof(CanParse)} was.");
        }

        _rawValue = InitialValueParse.Invoke(Model.Value);

        EnsureClassNames();

        if(Model.Validity == InputValidityType.Invalid && ErrorTitleParse != null)
        {
            var errorTitle = ErrorTitleParse(Model.ErrorValue);
            _ = Attributes.TryAdd("title", errorTitle);
        }

        _bindEvent = UpdateOnFocusLost ? "onchange" : "oninput";

        _valueParseSubscription = CanParse == null ?
            _rawValueReceiver.Subscribe(v => _rawValue = v) :
            _rawValueReceiver.Subscribe(v =>
            {
                if(CanParse(v))
                {
                    Model.Value = Parse!.Invoke(v);
                    _rawValue = v;
                }
            });
    }

    private void EnsureClassNames()
    {
        var validityClasses = ( Model.Validity switch
        {
            InputValidityType.Invalid => Style.InvalidStyle,
            InputValidityType.Valid => Style.ValidStyle,
            _ => Style.NoneValidityStyle
        } ).Classes;

        EnsureClassNames(validityClasses);
    }
}
