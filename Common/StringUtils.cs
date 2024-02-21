namespace RhoMicro.ApplicationFramework.Common;
using System;

/// <summary>
/// Contains utility functions for instances of <see cref="String"/>.
/// </summary>
public static class StringUtils
{
    /// <summary>
    /// The default ellipsis threshold length.
    /// </summary>
    public const Int32 MaxEllipsisLength = 10;
    /// <summary>
    /// Gets the default ellipsis.
    /// </summary>
    public const String Ellipsis = "...";
    /// <summary>
    /// Creates an ellipsis
    /// </summary>
    /// <param name="value"></param>
    /// <param name="ellipsis"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static String CreateEllipsis(String value, String ellipsis = Ellipsis, Int32 maxLength = MaxEllipsisLength)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(ellipsis);

        var maxLengthHalf = maxLength / 2;
        var result = value.Length - ellipsis.Length > maxLength ?
            $"{value[..maxLengthHalf]}{ellipsis}{value[^maxLengthHalf..]}" :
            value;

        return result;
    }
}
