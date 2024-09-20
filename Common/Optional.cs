namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents the optional monad, able to represent either a 
/// value of type <typeparamref name="T"/> (<c>Some</c>) or 
/// <see cref="Unit"/> (<c>None</c>).
/// </summary>
/// <typeparam name="T">The type of value represented.</typeparam>
[UnionType<Unit>(Alias = "None")]
public readonly partial struct Optional<[UnionType(Alias = "Some")] T>
{
    /// <summary>
    /// Applies a projection to the underlying value if this instance is
    /// <c>Some</c>; otherwise yields <c>None</c>.
    /// </summary>
    /// <typeparam name="TResult">The type of result yielded by <paramref name="projection"/>.</typeparam>
    /// <param name="projection">The projection to bind against.</param>
    /// <returns>
    /// The result of applying <paramref name="projection"/> to the underlying value 
    /// if this instance is <c>Some</c>; otherwise <c>None</c>.
    /// </returns>
    public Optional<TResult> Bind<TResult>(Func<T, TResult> projection)
    {
        ArgumentNullException.ThrowIfNull(projection);

        Optional<TResult> result = IsSome
                ? projection.Invoke(AsSome)
                : new Unit();

        return result;
    }
}
/// <summary>
/// Provides unit functions for the <see cref="Optional{T}"/> type.
/// </summary>
public static class Optional
{
    /// <summary>
    /// Creates a new optional with the underlying value provided.
    /// </summary>
    /// <param name="value">The value to initialize the new optional with.</param>
    /// <returns>A new optional representing <paramref name="value"/> (<c>Some</c>).</returns>
    public static Optional<T> Some<T>(T value) => value;
    /// <summary>
    /// Creates a new optional with no underlying value.
    /// </summary>
    /// <returns>A new optional representing <see cref="Unit"/> (<c>None</c>).</returns>
    public static Optional<T> None<T>() => new Unit();
}
