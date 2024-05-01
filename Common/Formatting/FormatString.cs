namespace RhoMicro.ApplicationFramework.Common;

using System.Text;

/// <summary>
/// Represents a format string; that is, a format string that contains placeholder holes.
/// </summary>
public sealed class FormatString : IEquatable<FormatString?>
{
    internal FormatString(
        IReadOnlyDictionary<FormatStringParameter, IReadOnlyList<FormatStringPosition>> namePositionsMap,
        IReadOnlyList<FormatStringPart> parts)
    {
        _namePositionsMap = namePositionsMap;
        Parts = parts;
    }

    private readonly IReadOnlyDictionary<FormatStringParameter, IReadOnlyList<FormatStringPosition>> _namePositionsMap;

    /// <summary>
    /// Gets all parts (placeholders and literals)
    /// </summary>
    public IReadOnlyList<FormatStringPart> Parts { get; }

    /// <summary>
    /// Implicitly converts a source format string to a <see cref="FormatString"/>.
    /// </summary>
    /// <param name="sourceString"></param>
    public static implicit operator FormatString(String sourceString) => new FormatStringBuilder().Add(sourceString).Build();
    /// <summary>
    /// Implicitly converts a <see cref="FormatString"/> to a source format string.
    /// </summary>
    /// <param name="formatString"></param>
    public static implicit operator String(FormatString formatString)
    {
        ArgumentNullException.ThrowIfNull(formatString);

        return formatString.ToSourceString();
    }

    /// <summary>
    /// Gets the source <see cref="String"/> representing this format string.
    /// <para>
    /// For example, a format string with two placeholders will result in a source <see cref="String"/> structured like so:
    /// </para>
    /// <para>
    /// <c>Hell</c>, <c>PH</c>, <c>, W</c>, <c>PH</c>, <c>!</c>
    /// </para>
    /// <para>
    /// <c>Hell{0}, W{1}!</c>
    /// </para>
    /// </summary>
    /// <returns>
    /// The source <see cref="String"/> representing this format string.
    /// </returns>
    public String ToSourceString()
    {
        if(Parts is not [{ }])
            return String.Empty;

        var resultBuilder = new StringBuilder();
        var i = 0;
        for(var j = 0; j < Parts.Count; j++)
        {
            Parts[j].Switch(
                str => resultBuilder.Append(str),
                ph =>
                {
                    _ = resultBuilder.Append('{').Append(i).Append('}');
                    i++;
                });
        }

        var result = resultBuilder.ToString();

        return result;
    }
    /// <summary>
    /// Substitutes the positional arguments passed for the placeholders required and returns the resulting string.
    /// </summary>
    /// <param name="arguments">
    /// The arguments to substitute for placeholders.
    /// </param>
    /// <returns>
    /// The formatted string.
    /// </returns>
    public String ToString(params FormatStringArgument[] arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        if(Parts is null)
            return String.Empty;

        var argsMap = MapArguments(arguments);
        var result = StringifyFormat(argsMap);

        return result;
    }

    private String StringifyFormat(IReadOnlyDictionary<FormatStringPosition, FormatStringArgument> argsMap)
    {
        var resultBuilder = new StringBuilder();
        for(var i = 0; i < Parts.Count; i++)
        {
            Parts[i].Switch(
                str => resultBuilder.Append(str),
                ph => resultBuilder.Append(argsMap[i].Value));
        }

        var result = resultBuilder.ToString();
        return result;
    }

    private IReadOnlyDictionary<FormatStringPosition, FormatStringArgument> MapArguments(FormatStringArgument[] arguments)
    {
        var argMapPosition = new Dictionary<FormatStringPosition, FormatStringArgument>();

        foreach(var arg in arguments)
        {
            arg.PositionOrParameter.Switch(
                mapPosition,
                name =>
                {
                    if(!_namePositionsMap.TryGetValue(name, out var positions))
                        throw new ArgumentException($"An argument with the name {name} could not be located in the format string.", nameof(arguments));

                    positions.ForEach(mapPosition);
                });

            void mapPosition(FormatStringPosition position)
            {
                if(!argMapPosition.TryAdd(position, arg))
                    throw new ArgumentException($"An argument with the position {position} has already been defined.", nameof(arguments));
            }
        }

        return argMapPosition;
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public override String ToString() => ToSourceString();
    public override Boolean Equals(Object? obj) => Equals(obj as FormatString);
    public Boolean Equals(FormatString? other) => other is not null && Parts.SequenceEqual(other.Parts);
    public override Int32 GetHashCode() => Parts.Aggregate(new HashCode(), (hc, p) =>
    {
        hc.Add(p);
        return hc;
    }).ToHashCode();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

