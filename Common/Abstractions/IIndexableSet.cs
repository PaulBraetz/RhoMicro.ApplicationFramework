namespace RhoMicro.ApplicationFramework.Common.Abstractions;

using System.Collections.Generic;

/// <summary>
/// Represents a set of elements that are accessible via index.
/// </summary>
/// <typeparam name="T">The type of elements contained.</typeparam>
/// <typeparam name="TSelf">The implementing type.</typeparam>
public interface IIndexableSet<TSelf, T> : IReadOnlyList<T>, IEquatable<TSelf>
    where T : notnull
    where TSelf : IIndexableSet<TSelf, T>
{
    /// <summary>
    /// Determines whether the set contains a specific value.
    /// </summary>
    /// <param name="element">The object to locate in the set.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="element"/> is found in the set; otherwise, <see langword="false"/>.
    /// </returns>
    Boolean Contains(T element);
    /// <summary>
    /// Searches for the specified object and returns the zero-based index its occurrence.
    /// </summary>
    /// <param name="element">
    /// The object to locate in the set.
    /// </param>
    /// <returns>
    /// The zero-based index of <paramref name="element"/> within the set, if found; otherwise, -1.
    /// </returns>
    Int32 IndexOf(T element);

    /// <summary>
    /// Creates a modified copy of the set with <paramref name="elementsToAdd"/> added and <paramref name="elementsToRemove"/> removed.
    /// </summary>
    /// <param name="elementsToRemove">The elements to ensure are not present in the resulting set.</param>
    /// <param name="elementsToAdd">The elements to ensure are present in the resulting set.</param>
    /// <returns>A new modified copy of the set containing <paramref name="elementsToAdd"/> but not <paramref name="elementsToRemove"/>.</returns>
    TSelf Mutate(IEnumerable<T> elementsToAdd, IEnumerable<T> elementsToRemove);

    /// <summary>
    /// Creates a modified copy of the set with <paramref name="elements"/> removed.
    /// </summary>
    /// <param name="elements">The elements to ensure are not present in the resulting set.</param>
    /// <returns>A new modified copy of the set without <paramref name="elements"/>.</returns>
    TSelf Remove(IEnumerable<T> elements);
    /// <summary>
    /// Creates a modified copy of the set with <paramref name="element"/> removed.
    /// </summary>
    /// <param name="element">The element to ensure is not present in the resulting set.</param>
    /// <returns>A new modified copy of the set without <paramref name="element"/> or this instance if it is not present in the set.</returns>
    TSelf Remove(T element);

    /// <summary>
    /// Creates a modified copy of the set with <paramref name="elements"/> added.
    /// </summary>
    /// <param name="elements">The elements to ensure are present in the resulting set.</param>
    /// <returns>A new modified copy of the set with <paramref name="elements"/>.</returns>
    TSelf Add(IEnumerable<T> elements);
    /// <summary>
    /// Creates a modified copy of the set with <paramref name="element"/> added.
    /// </summary>
    /// <param name="element">The element to ensure is present in the resulting set.</param>
    /// <returns>A new modified copy of the set with <paramref name="element"/> or this instance if it is already present in the set.</returns>
    TSelf Add(T element);
}
