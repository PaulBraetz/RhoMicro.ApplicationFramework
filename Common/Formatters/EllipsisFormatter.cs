namespace RhoMicro.ApplicationFramework.Common.Formatters;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Ellipsis based formatter for <see cref="String"/>.
/// </summary>
public sealed class EllipsisFormatter : IStaticFormatter<String>
{
    /// <summary>
    /// Initializes a new instance. If possible, use <see cref="Instance"/> instead.
    /// </summary>
    public EllipsisFormatter() { }
    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static EllipsisFormatter Instance { get; } = new();

    private const Int32 _ellipsisLimit = 10;
    private const Int32 _ellipsisLimitHalf = _ellipsisLimit / 2;

    /// <inheritdoc/>
    public String Format(String value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var result = value.Length > _ellipsisLimit ?
            $"{value[.._ellipsisLimitHalf]} [...] {value[^_ellipsisLimitHalf..]}" :
            value;

        return result;
    }
}
