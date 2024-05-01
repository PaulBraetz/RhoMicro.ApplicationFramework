namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.CodeAnalysis;

//"Hello, {world}!" -> FormatString fs -> fs.ToString("World") -> "Hello, World!"

/// <summary>
/// Represents a format string part; that is, either a literal or a named placeholder.
/// </summary>
[UnionType<FormatStringLiteral, FormatStringParameter>(Storage = StorageOption.Field)]
public readonly partial struct FormatStringPart
{
    /// <inheritdoc/>
    public override String ToString() => Match(l => l.ToString(), p => p.ToString());
}

