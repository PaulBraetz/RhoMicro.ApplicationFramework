namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents a format string literal part.
/// </summary>
[UnionType<String, Char>]
public readonly partial struct FormatStringLiteral
{
    /// <inheritdoc/>
    public override String ToString() => Match(s => s, c => new(c, 1));
}

