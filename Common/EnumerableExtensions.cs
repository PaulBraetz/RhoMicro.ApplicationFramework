namespace RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Contains extensions for the <see cref="IEnumerable{T}"/> type.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Applies a <c>TryXXX</c> factory to all elements in a sequence of values and yields those that produce a value.
    /// </summary>
    /// <typeparam name="TParameter">The type of parameter taken.</typeparam>
    /// <typeparam name="TResult">The type of result produced.</typeparam>
    /// <param name="values">The input sequence to project.</param>
    /// <param name="factory">The <c>TryXXX</c> factory to apply to elements of <paramref name="values"/>.</param>
    /// <returns>The projected sequence, containing elements produced by <paramref name="factory"/>.</returns>
    public static IEnumerable<TResult> TrySelect<TParameter, TResult>(this IEnumerable<TParameter> values, TryFactory<TParameter, TResult> factory)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(factory);

        foreach(var value in values)
        {
            if(factory.Invoke(value, out var result))
            {
                yield return result;
            }
        }
    }
    /// <summary>
    /// Enumerates the values of an enumeration while checking a cancellation token for cancellation.
    /// </summary>
    /// <typeparam name="T">The type of value in <paramref name="enumeration"/>.</typeparam>
    /// <param name="enumeration">The enumeration to iterate over.</param>
    /// <param name="cancellationToken">The cancellation token to check for cancellation.</param>
    /// <returns></returns>
    public static IEnumerable<T> WithCancellation<T>(this IEnumerable<T> enumeration, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(enumeration);

        cancellationToken.ThrowIfCancellationRequested();

        foreach(var value in enumeration)
        {
            yield return value;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}