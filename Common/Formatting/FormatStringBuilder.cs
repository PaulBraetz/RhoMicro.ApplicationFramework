namespace RhoMicro.ApplicationFramework.Common;

using System.Text.RegularExpressions;

/// <summary>
/// Builder for instances of <see cref="FormatString"/>.
/// </summary>
public sealed partial class FormatStringBuilder
{
    private readonly List<FormatStringPart> _parts = [];
    private readonly List<FormatStringParameter> _parameters = [];
    private readonly Dictionary<FormatStringParameter, List<FormatStringPosition>> _namePositionsMapMutable = [];
    private readonly Dictionary<FormatStringParameter, IReadOnlyList<FormatStringPosition>> _namePositionsMap = [];

    [GeneratedRegex(@"(?<=[^{]?{)[^{}]*(?=}[^}]?)")]
    private static partial Regex GetParameterPattern();

    /// <summary>
    /// Clears the builder.
    /// </summary>
    /// <returns>A reference to the builder; for chaining of further methods.</returns>
    public FormatStringBuilder Clear()
    {
        _parts.Clear();
        _parameters.Clear();
        _namePositionsMap.Clear();
        _namePositionsMapMutable.Clear();

        return this;
    }

    /// <summary>
    /// Parses and adds a source format string to the builder.
    /// </summary>
    /// <param name="sourceFormatString">The source format string to parse and add.</param>
    /// <returns>A reference to the builder; for chaining of further methods.</returns>
    public FormatStringBuilder Add(String sourceFormatString)
    {
        ArgumentNullException.ThrowIfNull(sourceFormatString);

        var pattern = GetParameterPattern();
        var matches = pattern.EnumerateMatches(sourceFormatString);
        Int32 openBraceIndex, closeBraceIndex, lastCloseBraceIndex = 0;
        foreach(var match in matches)
        {
            openBraceIndex = match.Index - 1;
            closeBraceIndex = match.Index + match.Length + 1;

            var distanceToLastClosingBrace = openBraceIndex - lastCloseBraceIndex;

            if(distanceToLastClosingBrace > 1)
            {
                FormatStringLiteral literal = sourceFormatString.Substring(lastCloseBraceIndex, distanceToLastClosingBrace);
                _ = Add(literal);
            }

            var parameter = sourceFormatString.Substring(match.Index, match.Length);
            _ = AddParameter(parameter);
            lastCloseBraceIndex = closeBraceIndex;
        }

        var remainingLiteralLength = sourceFormatString.Length - ( lastCloseBraceIndex + 1 );
        if(remainingLiteralLength == 1)
        {
            FormatStringLiteral literal = sourceFormatString[^1];
            _ = AddLiteral(literal);
        } else if(remainingLiteralLength > 1)
        {
            FormatStringLiteral literal = sourceFormatString[lastCloseBraceIndex..];
            _ = AddLiteral(literal);
        }

        return this;
    }
    /// <summary>
    /// Adds a format string part to the builder.
    /// </summary>
    /// <param name="part">The part to add.</param>
    /// <returns>A reference to the builder; for chaining of further methods.</returns>
    public FormatStringBuilder Add(FormatStringPart part) => part.Match(AddLiteral, AddParameter);
    /// <summary>
    /// Adds a literal part to the builder.
    /// </summary>
    /// <param name="literal">The literal to add.</param>
    /// <returns>A reference to the builder; for chaining of further methods.</returns>
    public FormatStringBuilder AddLiteral(FormatStringLiteral literal)
    {
        _parts.Add(literal);

        return this;
    }
    /// <summary>
    /// Adds a parameter part to the builder.
    /// </summary>
    /// <param name="parameter">The parameter to add.</param>
    /// <returns>A reference to the builder; for chaining of further methods.</returns>
    public FormatStringBuilder AddParameter(FormatStringParameter parameter)
    {
        if(!_namePositionsMapMutable.TryGetValue(parameter, out var positions))
        {
            positions = [];
            _namePositionsMap[parameter] = positions;
            _namePositionsMapMutable[parameter] = positions;
        }

        positions.Add(_parts.Count);
        _parts.Add(parameter);
        _parameters.Add(parameter);

        return this;
    }

    /// <summary>
    /// Builds a new <see cref="FormatString"/>.
    /// </summary>
    /// <returns>A new <see cref="FormatString"/>.</returns>
    public FormatString Build() => new(_namePositionsMap.ToDictionary(), _parts.ToList()); //freeze via copy
    /// <inheritdoc/>
    public override String ToString() => Build().ToString();
}

