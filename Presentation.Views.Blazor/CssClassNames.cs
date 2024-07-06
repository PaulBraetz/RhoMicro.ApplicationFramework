namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Represents a set of css class names.
/// </summary>
public sealed class CssClassNames : IIndexableSet<CssClassNames, CssClassName>
{
    private CssClassNames(IndexableSet<CssClassName> classNames) => _classNames = classNames;

    private readonly IndexableSet<CssClassName> _classNames;

    /// <summary>
    /// Gets an empty set of css class names.
    /// </summary>
    public static CssClassNames Empty { get; } = new(IndexableSet<CssClassName>.Empty);

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

        var strongClassNames = classNames.Select(n => (CssClassName)n);

        return Create(strongClassNames);
    }
    /// <summary>
    /// Creates a new css class name set from a collection of class names.
    /// </summary>
    /// <param name="classNames">The class names to include in the set.</param>
    /// <returns>A new css class name set containing all distinct non-empty class names from <paramref name="classNames"/>.</returns>
    public static CssClassNames Create(IEnumerable<CssClassName> classNames)
    {
        ArgumentNullException.ThrowIfNull(classNames);

        var classNamesSet = IndexableSet.Create(
            elements: classNames.Split(),
            acceptElement: className => className.Length > 0);

        return new(classNamesSet);
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public Int32 Count => _classNames.Count;
    public CssClassName this[Int32 index] => _classNames[index];

    public Boolean Contains(CssClassName element) => _classNames.Contains(element);
    public Int32 IndexOf(CssClassName element) => _classNames.IndexOf(element);

    public CssClassNames Mutate(IEnumerable<CssClassName> elementsToAdd, IEnumerable<CssClassName> elementsToRemove) => new(_classNames.Mutate(elementsToAdd.Split(), elementsToRemove.Split()));

    public CssClassNames Remove(IEnumerable<CssClassName> elements) => new(_classNames.Remove(elements.Split()));
    public CssClassNames Remove(CssClassName element) => new(_classNames.Remove(element.Split()));

    public CssClassNames Add(IEnumerable<CssClassName> elements) => new(_classNames.Add(elements.Split()));
    public CssClassNames Add(CssClassName element) => new(_classNames.Add(element.Split()));

    public IEnumerator<CssClassName> GetEnumerator() => _classNames.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)_classNames ).GetEnumerator();

    public Boolean Equals(CssClassNames? other) => other is { } && _classNames.Equals(other._classNames);
    public override Boolean Equals(Object? obj) => Equals(obj as CssClassNames);

    public override Int32 GetHashCode() => _classNames.GetHashCode();

    public override String ToString() => String.Join(' ', _classNames);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

file static class Extensions
{
    private static String[] Split(this UnconditionalCssClassName name) =>
        name.Match(s => s.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));
    public static IEnumerable<CssClassName> Split(this CssClassName cssClassName)
    {
        var parts = cssClassName.Match(
                unconditional => unconditional.Split(),
                conditional => conditional.Name.Split());

        foreach(var part in parts)
        {
            yield return cssClassName.Match<CssClassName>(
                unconditional => part,
                conditional => (part, conditional.Condition));
        }
    }
    public static IEnumerable<CssClassName> Split(this IEnumerable<CssClassName> cssClassNames) => cssClassNames.SelectMany(Split);
}
