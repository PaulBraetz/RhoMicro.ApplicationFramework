namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IOptionModelFactory{T}"/>. Ids of option models created will be auto-generated.
/// </summary>
/// <typeparam name="T">The type of value to be represented by options.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="nameFormatter">The formatter to use when obtaining a new options name.</param>
public sealed class OptionModelFactory<T>(IStaticFormatter<T> nameFormatter) : IOptionModelFactory<T>
{
    /// <inheritdoc/>
    public IOptionModel<T> Create(T value, Boolean disabled = false, Boolean isSelected = false)
    {
        var id = Guid.NewGuid().ToString();
        var name = nameFormatter.Format(value);
        var result = new OptionModel<T>(value, disabled, isSelected, id, name);

        return result;
    }
}
