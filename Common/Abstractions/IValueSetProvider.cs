namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a generic provider of a set of values.
/// </summary>
/// <typeparam name="T">The type of values to provide.</typeparam>
public interface IValueSetProvider<T>
{
    /// <summary>
    /// Provides the set of values.
    /// </summary>
    /// <returns>The set of values.</returns>
    IReadOnlySet<T> GetValues();
}
