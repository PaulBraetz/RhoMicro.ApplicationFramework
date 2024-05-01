namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Strategy based implementation of <see cref="IStaticFormatter{T}"/>.
/// </summary>
/// <typeparam name="T">The type of Object to format.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="strategy">The strategy used when invoking <see cref="Format(T)"/>.</param>
public sealed class FormatterStrategy<T>(Func<T, String> strategy) : IStaticFormatter<T>
{
    /// <inheritdoc/>
    public String Format(T value)
    {
        var result = strategy.Invoke(value);

        return result;
    }
}
