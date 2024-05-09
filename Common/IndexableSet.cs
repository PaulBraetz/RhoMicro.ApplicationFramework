namespace RhoMicro.ApplicationFramework.Common;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml.Linq;

/// <summary>
/// Represents a set of elements that are accessible via index.
/// </summary>
/// <typeparam name="T">The type of elements contained.</typeparam>
public sealed class IndexableSet<T> : IIndexableSet<IndexableSet<T>, T>
    where T : notnull
{
    internal IndexableSet(ImmutableList<T> elements, ImmutableDictionary<T, Int32> indices, IEqualityComparer<T> comparer, Func<T, T>? projectElement, Func<T, Boolean>? acceptElement)
    {
        _elements = elements;
        _indices = indices;
        _comparer = comparer;
        _projectElement = projectElement;
        _acceptElement = acceptElement;
    }

    private readonly IEqualityComparer<T> _comparer;
    private readonly ImmutableList<T> _elements;
    private readonly ImmutableDictionary<T, Int32> _indices;
    private readonly Func<T, T>? _projectElement;
    private readonly Func<T, Boolean>? _acceptElement;

    /// <summary>
    /// Gets an empty set.
    /// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static IndexableSet<T> Empty { get; } = new([], ImmutableDictionary<T, Int32>.Empty, EqualityComparer<T>.Default, null, null);
#pragma warning restore CA1000 // Do not declare static members on generic types

    /// <inheritdoc/>
    public Boolean Contains(T element) => Count > 0 && !Reject(ref element) && _indices.ContainsKey(element);
    /// <inheritdoc/>
    public Int32 IndexOf(T element) => ( Count > 0 && !Reject(ref element) && _indices.TryGetValue(element, out var index) ) ? index : -1;

    private IndexableSet<T> WithElements(ImmutableList<T> elements, ImmutableDictionary<T, Int32> indices) =>
        new(elements, indices, _comparer, _projectElement, _acceptElement);
    private IndexableSet<T> WithElements(ImmutableList<T>.Builder elements, ImmutableDictionary<T, Int32>.Builder indices) =>
        IndexableSetHelpers.Create(elements, indices, _comparer, _projectElement, _acceptElement);

    private Boolean Reject(ref T element)
    {
        element = _projectElement != null
            ? _projectElement.Invoke(element)
            : element;
        var result = _acceptElement != null && !_acceptElement.Invoke(element);

        return result;
    }
    private Boolean Reject(ref IEnumerable<T> elements)
    {
        elements = _projectElement != null
            ? elements.Select(_projectElement)
            : elements;
        elements = _acceptElement != null
            ? elements.Where(_acceptElement)
            : elements;
        var result = !elements.Any();

        return result;
    }

    #region Mutate
    /// <inheritdoc/>
    public IndexableSet<T> Mutate(IEnumerable<T> elementsToAdd, IEnumerable<T> elementsToRemove)
    {
        ArgumentNullException.ThrowIfNull(elementsToAdd);
        ArgumentNullException.ThrowIfNull(elementsToRemove);

        if(Reject(ref elementsToAdd) & Reject(ref elementsToRemove))
            return this;

        var modifiedElements = _elements.ToBuilder();
        var modifiedIndices = _indices.ToBuilder();

        AddElements(elementsToAdd, modifiedElements, modifiedIndices);
        RemoveElements(elementsToRemove, modifiedElements, modifiedIndices);

        var result = WithElements(modifiedElements, modifiedIndices);

        return result;
    }
    #endregion
    #region Remove
    /// <inheritdoc/>
    public IndexableSet<T> Remove(T element)
    {
        if(Count == 0 || Reject(ref element) || !_indices.TryGetValue(element, out var index))
            return this;

        var modifiedIndices = _indices.Remove(element);
        var modifiedElements = _elements.RemoveAt(index);

        var result = WithElements(modifiedElements, modifiedIndices);

        return result;
    }
    /// <inheritdoc/>
    public IndexableSet<T> Remove(IEnumerable<T> elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        if(Count == 0 || Reject(ref elements))
            return this;

        var modifiedElements = _elements.ToBuilder();
        var modifiedIndices = _indices.ToBuilder();

        RemoveElements(elements, modifiedElements, modifiedIndices);

        var result = WithElements(modifiedElements, modifiedIndices);

        return result;
    }

    private static void RemoveElements(IEnumerable<T> elements, ImmutableList<T>.Builder modifiedElements, ImmutableDictionary<T, Int32>.Builder modifiedIndices)
    {
        var indicesToRemove = elements
            .Select(e => (removed: modifiedIndices.Remove(e, out var i), index: i))
            .Where(t => t.removed)
            .Select(t => t.index)
            .OrderDescending();

        foreach(var index in indicesToRemove)
        {
            modifiedElements.RemoveAt(index);
        }
    }
    #endregion
    #region Add
    /// <inheritdoc/>
    public IndexableSet<T> Add(T element)
    {
        if(Reject(ref element) || _indices.ContainsKey(element))
            return this;

        var modifiedElements = _elements.Add(element);
        var modifiedIndices = _indices.Add(element, modifiedElements.Count - 1);

        var result = WithElements(modifiedElements, modifiedIndices);

        return result;
    }
    /// <inheritdoc/>
    public IndexableSet<T> Add(IEnumerable<T> elements)
    {
        ArgumentNullException.ThrowIfNull(elements);

        if(Reject(ref elements))
            return this;

        var modifiedElements = _elements.ToBuilder();
        var modifiedIndices = _indices.ToBuilder();

        AddElements(elements, modifiedElements, modifiedIndices);

        var result = WithElements(modifiedElements, modifiedIndices);

        return result;
    }

    private static void AddElements(IEnumerable<T> elements, ImmutableList<T>.Builder modifiedElements, ImmutableDictionary<T, Int32>.Builder modifiedIndices)
    {
        foreach(var element in elements)
        {
            if(modifiedIndices.TryAdd(element, modifiedElements.Count))
                modifiedElements.Add(element);
        }
    }
    #endregion
    #region Overrides & Interface Implementations
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public T this[Int32 index] => ( (IReadOnlyList<T>)_elements )[index];
    public Int32 Count => ( (IReadOnlyCollection<T>)_elements ).Count;
    public IEnumerator<T> GetEnumerator() => ( (IEnumerable<T>)_elements ).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)_elements ).GetEnumerator();
    public override Boolean Equals(Object? obj) => Equals(obj as IndexableSet<T>);
    public Boolean Equals(IndexableSet<T>? other)
    {
        if(other is null || Count != other.Count)
            return false;

        if(Count == 0)
            return true;

        foreach(var (element, otherIndex) in other._indices)
        {
            if(!_indices.TryGetValue(element, out var index) || index != otherIndex)
                return false;
        }

        return true;
    }
    public override Int32 GetHashCode()
    {
        var hc = new HashCode();
        hc.Add(Count);

        foreach(var (element, index) in _indices)
        {
            hc.Add(element, _comparer);
            hc.Add(index);
        }

        var result = hc.ToHashCode();

        return result;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion
}

/// <summary>
/// Contains factory methods for <see cref="IndexableSet{T}"/>.
/// </summary>
public static class IndexableSet
{
    /// <summary>
    /// Creates a new indexable set from a collection of class names.
    /// </summary>
    /// <param name="elements">The elements to include in the set.</param>
    /// <param name="comparer">The comparer to use when comparing elements to add to the set.</param>
    /// <param name="projectElement">The optional projection to apply when adding to or removing elements from the set.</param>
    /// <param name="acceptElement">The optional predicate to determine if an (optionally projected) element should be able to be added to or removed elements from the set.</param>
    /// <returns>A new indexable set containing all distinct elements from <paramref name="elements"/>.</returns>
    public static IndexableSet<T> Create<T>(
        IEnumerable<T> elements,
        IEqualityComparer<T>? comparer = null,
        Func<T, T>? projectElement = null,
        Func<T, Boolean>? acceptElement = null)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(elements);

        comparer ??= EqualityComparer<T>.Default;

        var elementsBuilder = ImmutableList.CreateBuilder<T>();
        var indicesBuilder = ImmutableDictionary.CreateBuilder<T, Int32>(comparer);

        var projectedElements = projectElement != null
            ? elements.Select(projectElement)
            : elements;
        var acceptedElements = acceptElement != null
            ? projectedElements.Where(acceptElement)
            : projectedElements;

        foreach(var element in acceptedElements)
        {
            var index = elementsBuilder.Count;
            if(!indicesBuilder.TryAdd(element, index))
                continue;

            elementsBuilder.Add(element);
        }

        return IndexableSetHelpers.Create(elementsBuilder, indicesBuilder, comparer, projectElement, acceptElement);
    }
}

file static class IndexableSetHelpers
{
    public static IndexableSet<T> Create<T>(
        ImmutableList<T>.Builder elementsBuilder,
        ImmutableDictionary<T, Int32>.Builder indicesBuilder,
        IEqualityComparer<T> comparer,
        Func<T, T>? projectElement,
        Func<T, Boolean>? acceptElement)
        where T : notnull
    {
        var elements = elementsBuilder.ToImmutable();
        var indices = indicesBuilder.ToImmutable();

        var result = new IndexableSet<T>(
            elements,
            indices,
            comparer,
            projectElement,
            acceptElement);

        return result;
    }
}