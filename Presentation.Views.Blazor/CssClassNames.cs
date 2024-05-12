namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System.Collections;
using System.Collections.Generic;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a set of css class names.
/// </summary>
public sealed class CssClassNames : IIndexableSet<CssClassNames, String>
{
    private CssClassNames(IndexableSet<String> classNames)
    {
        _classNames = classNames;
        _stringRepresentation = new(() => String.Join(' ', classNames));
    }

    private readonly IndexableSet<String> _classNames;
    private readonly Lazy<String> _stringRepresentation;

    /// <summary>
    /// Gets an empty set of css class names.
    /// </summary>
    public static CssClassNames Empty { get; } = new(IndexableSet<String>.Empty);

    /// <summary>
    /// Creates a new css class name set from an Object. The following types are checked for:
    /// <list type="bullet">
    /// <item>an existing <see cref="CssClassNames"/>,</item>
    /// <item>a <see cref="String"/>,</item>
    /// <item>an <see cref="IEnumerable{T}"/> of <see cref="String"/> or</item>
    /// <item>an <see cref="IEnumerable"/>.</item>
    /// </list>
    /// If <paramref name="classNames"/> does not match any of the types outlined, a string representation of 
    /// <paramref name="classNames"/> will be used for determining css class names instead.
    /// </summary>
    /// <param name="classNames">The class names to include in the set.</param>
    /// <returns>A new css class name set containing all distinct non-empty class names from <paramref name="classNames"/>.</returns>
    public static CssClassNames Create(Object? classNames)
    {
        var result = classNames switch
        {
            CssClassNames set => set,
            String str => Create(str),
            IEnumerable<String> specificCollection => Create(specificCollection),
            IEnumerable generalCollection => Create(generalCollection.OfType<Object>().Select(n => n.ToString() ?? String.Empty)),
            null => Empty,
            _ => Create(classNames.ToString() ?? String.Empty)
        };

        return result;
    }
    /// <summary>
    /// Creates a new css class name set from a string of space-delimited class names.
    /// </summary>
    /// <param name="classNames">The class names to include in the set.</param>
    /// <returns>A new css class name set containing all distinct non-empty class names from <paramref name="classNames"/>.</returns>
    public static CssClassNames Create(String classNames)
    {
        ArgumentNullException.ThrowIfNull(classNames);

        var splitClassNames = classNames.Split(' ');

        var result = Create(splitClassNames);

        return result;
    }
    /// <summary>
    /// Creates a new css class name set from a collection of class names.
    /// </summary>
    /// <param name="classNames">The class names to include in the set.</param>
    /// <returns>A new css class name set containing all distinct non-empty class names from <paramref name="classNames"/>.</returns>
    public static CssClassNames Create(IEnumerable<String> classNames)
    {
        ArgumentNullException.ThrowIfNull(classNames);

        var classNamesSet = IndexableSet.Create(
            elements: classNames,
            projectElement: className => className.Trim(),
            acceptElement: className => className.Length > 0);

        return new(classNamesSet);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public Int32 Count => _classNames.Count;
    public String this[Int32 index] => _classNames[index];
    public Boolean Contains(String element) => _classNames.Contains(element);
    public Int32 IndexOf(String element) => _classNames.IndexOf(element);
    public CssClassNames Mutate(IEnumerable<String> elementsToAdd, IEnumerable<String> elementsToRemove) => new(_classNames.Mutate(elementsToAdd, elementsToRemove));
    public CssClassNames Remove(IEnumerable<String> elements) => new(_classNames.Remove(elements));
    public CssClassNames Remove(String element) => new(_classNames.Remove(element));
    public CssClassNames Add(IEnumerable<String> elements) => new(_classNames.Add(elements));
    public CssClassNames Add(String element) => new(_classNames.Add(element));
    public IEnumerator<String> GetEnumerator() => _classNames.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)_classNames ).GetEnumerator();
    public Boolean Equals(CssClassNames? other) => other is { } && _classNames.Equals(other._classNames);
    public override Boolean Equals(Object? obj) => Equals(obj as CssClassNames);
    public override Int32 GetHashCode() => _classNames.GetHashCode();
    public override String ToString() => _stringRepresentation.Value;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
