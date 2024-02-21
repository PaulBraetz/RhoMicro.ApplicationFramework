namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Provides facilities for formatting objects without requiring format codes.
/// </summary>
/// <typeparam name="T">The type of Object to format.</typeparam>
public interface IStaticFormatter<T>
{
    /// <summary>
    /// Formats an Object.
    /// </summary>
    /// <param name="value">The Object to format.</param>
    /// <returns>A formatted String representation of the Object.</returns>
    String Format(T value);
}
