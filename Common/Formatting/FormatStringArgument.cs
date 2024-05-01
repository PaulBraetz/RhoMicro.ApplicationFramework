namespace RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Represents an argument to substitute for all parameters identified by a position or name.
/// </summary>
/// <param name="PositionOrParameter">The position or name of parameters to substitute for.</param>
/// <param name="Value">The value to substitute.</param>
public readonly record struct FormatStringArgument(FormatStringPositionOrParameter PositionOrParameter, String Value)
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static implicit operator FormatStringArgument((String parameter, String value) parameterAndValue)
        => new((FormatStringParameter)parameterAndValue.parameter, parameterAndValue.value);
    public static implicit operator FormatStringArgument((Int32 position, String value) parameterAndValue)
        => new((FormatStringPosition)parameterAndValue.position, parameterAndValue.value);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

