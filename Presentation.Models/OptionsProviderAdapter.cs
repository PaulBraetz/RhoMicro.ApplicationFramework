namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Adapts instances of <see cref="IValueSetProvider{T}"/> onto <see cref="IOptionsProvider{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class OptionsProviderAdapter<T> : IOptionsProvider<T>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="valueProvider">The slot provider to adapt onto the <see cref="IOptionsProvider{T}"/> interface.</param>
    /// <param name="valueOptionFactory">The factory to invoke when creating option models in <see cref="GetOptions"/>.</param>
    public OptionsProviderAdapter(IValueSetProvider<T> valueProvider, IOptionModelFactory<T> valueOptionFactory)
    {
        _valueProvider = valueProvider;
        _valueOptionFactory = valueOptionFactory;
    }

    private readonly IValueSetProvider<T> _valueProvider;
    private readonly IOptionModelFactory<T> _valueOptionFactory;

    /// <inheritdoc/>
    public IReadOnlySet<IOptionModel<T>> GetOptions()
    {
        var slots = _valueProvider.GetValues();
        var result = slots.Select((s, i) => _valueOptionFactory.Create(s, isSelected: i == 0))
                        .ToHashSet();
        return result;
    }
}
