namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Provides a set of options.
/// </summary>
/// <typeparam name="T">The type of option value provided.</typeparam>
public interface IOptionsProvider<T>
{
    /// <summary>
    /// Gets the set of options.
    /// </summary>
    /// <returns>The set of options.</returns>
    IReadOnlySet<IOptionModel<T>> GetOptions();
}
