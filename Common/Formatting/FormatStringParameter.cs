namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents a format string parameter in a format string.
/// </summary>
[UnionType<String>]
[UnionTypeSettings(ToStringSetting = ToStringSetting.None)]
public readonly partial struct FormatStringParameter
{
    /// <summary>
    /// Creates a new instance of <see cref="FormatStringParameter"/>.
    /// </summary>
    /// <param name="value">The value of the name returned.</param>
    /// <returns>The name created.</returns>
    public static FormatStringParameter Create([UnionTypeFactory] String value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        return new(value);
    }
    /// <inheritdoc/>
    public override String ToString() => $"{{{AsString}}}";
}

