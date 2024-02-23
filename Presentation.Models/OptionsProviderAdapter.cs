namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Adapts instances of <see cref="IValueSetProvider{T}"/> onto <see cref="IOptionsProvider{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="valueProvider">The slot provider to adapt onto the <see cref="IOptionsProvider{T}"/> interface.</param>
/// <param name="valueOptionFactory">The factory to invoke when creating option models in <see cref="GetOptions"/>.</param>
public sealed class OptionsProviderAdapter<T>(IValueSetProvider<T> valueProvider, IOptionModelFactory<T> valueOptionFactory)
    : IOptionsProvider<T>
{
    /// <inheritdoc/>
    public IReadOnlySet<IOptionModel<T>> GetOptions()
    {
        var slots = valueProvider.GetValues();
        var result = slots.Select((s, i) => valueOptionFactory.Create(s, isSelected: i == 0))
                        .ToHashSet();
        return result;
    }
}
