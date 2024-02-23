namespace RhoMicro.ApplicationFramework.Common.Formatters;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// <see cref="Object.ToString"/>-based implementation of <see cref="IStaticFormatter{T}"/>.
/// </summary>
/// <typeparam name="T">The type of object to format.</typeparam>
public sealed class ToStringFormatter<T> : IStaticFormatter<T>
{
    /// <summary>
    /// Gets a static instance.
    /// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types <- use is not common due to singleton di container
    public static ToStringFormatter<T> Instance { get; } = new();
#pragma warning restore CA1000 // Do not declare static members on generic types
    /// <inheritdoc/>
    public String Format(T value) => value?.ToString() ?? String.Empty;
}
