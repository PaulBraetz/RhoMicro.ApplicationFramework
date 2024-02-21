namespace RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Default implementation of <see cref="IOptionModelFactory{T}"/>. Ids of option models created will be auto-generated.
/// </summary>
/// <typeparam name="T">The type of value to be represented by options.</typeparam>
public sealed class OptionModelFactory<T> : IOptionModelFactory<T>
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="nameFormatter">The formatter to use when obtaining a new options name.</param>
    public OptionModelFactory(IStaticFormatter<T> nameFormatter)
    {
        _nameFormatter = nameFormatter;
    }
    private readonly IStaticFormatter<T> _nameFormatter;
    /// <inheritdoc/>
    public IOptionModel<T> Create(T value, Boolean disabled = false, Boolean isSelected = false)
    {
        var id = Guid.NewGuid().ToString();
        var name = _nameFormatter.Format(value);
        var result = new OptionModel<T>(value, disabled, isSelected, id, name);

        return result;
    }
}
