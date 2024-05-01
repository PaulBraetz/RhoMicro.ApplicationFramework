namespace RhoMicro.ApplicationFramework.Common;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents the option union, that is, an optional value or an instance of the <see cref="Unit"/> type.
/// </summary>
/// <typeparam name="T">The type of value to represent.</typeparam>
[UnionType<Unit>(Alias = "None")]
public readonly partial struct Maybe<[UnionType(Alias = "Some")] T>
{
    /// <summary>
    /// Gets the <see cref="Unit"/> instance of this <see cref="Maybe{T}"/>.
    /// </summary>
    public static Maybe<T> None { get; } = new Unit();
}
