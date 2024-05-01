namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents the position of a format string part in a format strings list of parts.
/// </summary>
[UnionType<Int32>]
[UnionTypeSettings(ToStringSetting = ToStringSetting.Simple)]
public readonly partial struct FormatStringPosition
{
    /// <summary>
    /// Creates a new instance of <see cref="FormatStringPosition"/>.
    /// </summary>
    /// <param name="value">The value of the position returned.</param>
    /// <returns>The position created.</returns>
    public static FormatStringPosition Create([UnionTypeFactory] Int32 value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);

        return new(value);
    }
}
