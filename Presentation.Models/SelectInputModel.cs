namespace RhoMicro.ApplicationFramework.Presentation.Models;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Provides factory method for creating instances of <see cref="SelectInputModel{TValue, TError}"/>.
/// </summary>
public static class SelectInputModel
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="options">The set of valid values supported by the model.</param>
    /// <param name="valueDefaultProviderFactory">The default value provider to use for initializing <see cref="SelectInputModel{TValue, TError}.Value"/>.</param>
    /// <param name="errorDefaultProvider">The default value provider to use for initializing <see cref="InputModel{TValue, TError}.ErrorValue"/>.</param>
    public static SelectInputModel<TValue, TError> Create<TValue, TError>(
        IReadOnlySet<IOptionModel<TValue>> options,
        IDefaultValueProviderFactory<IOptionModel<TValue>?> valueDefaultProviderFactory,
        IDefaultValueProvider<TError> errorDefaultProvider)
    {
        ArgumentNullException.ThrowIfNull(valueDefaultProviderFactory);
        ArgumentNullException.ThrowIfNull(options);

        var optionsMap = new Dictionary<String, IOptionModel<TValue>>();
        var selectedOption = options.FirstOrDefault(o => o.IsSelected);
        var valueDefaultProvider = valueDefaultProviderFactory.Create(() => selectedOption);
        var result = new SelectInputModel<TValue, TError>(
            optionsMap,
            valueDefaultProvider,
            errorDefaultProvider);

        foreach(var option in options)
        {
            option.Selected += result.OnSelected;
            optionsMap.Add(option.Id, option);
        }

        if(selectedOption != null)
        {
            if(result.Value != null)
                result.OnSelected(result.Value);
        }

        return result;
    }
}

/// <summary>
/// Implementation of <see cref="InputModel{TValue, TError}"/> where <c>TValue</c> 
/// is <see cref="IOptionModel{TValue}"/>.
/// </summary>
/// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
/// <typeparam name="TError">The type of error displayed by this model.</typeparam>
public sealed class SelectInputModel<TValue, TError> : InputModel<IOptionModel<TValue>?, TError>, ISelectInputModel<TValue, TError>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="options">The set of valid values supported by the model.</param>
    /// <param name="valueDefaultProvider">The default value provider to use for initializing <see cref="Value"/>.</param>
    /// <param name="errorDefaultProvider">The default value provider to use for initializing <see cref="InputModel{TValue, TError}.ErrorValue"/>.</param>
    internal SelectInputModel(
        IReadOnlyDictionary<String, IOptionModel<TValue>> options,
        IDefaultValueProvider<IOptionModel<TValue>?> valueDefaultProvider,
        IDefaultValueProvider<TError> errorDefaultProvider)
        : base(valueDefaultProvider, errorDefaultProvider)
        => _options = options;

    private readonly IReadOnlyDictionary<String, IOptionModel<TValue>> _options;
    /// <inheritdoc/>
    public override IOptionModel<TValue>? Value
    {
        get => base.Value;
        set
        {
            if(value != null && !_options.ContainsKey(value.Id))
                throw new ArgumentOutOfRangeException(nameof(value), value, "value must be contained in the set of valid alternatives.");

            OnSelected(value);
        }
    }
    internal void OnSelected(IOptionModel<TValue>? selected)
    {
        if(base.Value != null)
            base.Value.IsSelected = false;
        base.Value = selected;
    }
    /// <inheritdoc/>
    public IEnumerable<IOptionModel<TValue>> GetOptions() => _options.Values;
}
