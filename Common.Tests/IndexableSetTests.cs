namespace RhoMicro.ApplicationFramework.Common.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Contains tests for <see cref="IndexableSet{T}"/>
/// </summary>
public class IndexableSetTests
{
    /// <summary>
    /// Asserts that elements passed to <see cref="IndexableSet.Create{T}(IEnumerable{T}, IEqualityComparer{T}?, Func{T, T}?, Func{T, Boolean}?)"/> are present in the output set.
    /// </summary>
    /// <param name="elements"></param>
    [Theory]
    [InlineData([new[] { "a", "b", "c", "d" }])]
    [InlineData([new[] { "button", "myclass", "mx-3", "border" }])]
    public void CreatesWithElementsPassed(String[] elements)
    {
        var expected = elements.ToHashSet();
        var actual = IndexableSet.Create(elements);
        foreach(var element in actual)
            Assert.Contains(element, expected);
        Assert.Equal(actual.Count, expected.Count);
    }
    /// <summary>
    /// Asserts that elements passed to <see cref="IndexableSet.Create{T}(IEnumerable{T}, IEqualityComparer{T}?, Func{T, T}?, Func{T, Boolean}?)"/> are present in the output and distinct.
    /// </summary>
    /// <param name="elements"></param>
    [Theory]
    [InlineData([new[] { "a", "b", "b", "c", "d", "a" }])]
    [InlineData([new[] { "button", "border", "border", "myclass", "mx-3", "myClass", "border" }])]
    public void CreatesWithDistinctElementsPassed(String[] elements)
    {
        var expected = elements.ToHashSet();
        var actual = IndexableSet.Create(elements);
        foreach(var element in actual)
            Assert.Contains(element, expected);
        Assert.Equal(actual.Count, expected.Count);
    }
    /// <summary>
    /// Asserts that elements passed to <see cref="IndexableSet{T}.Add(T)"/> are present in the output.
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="elementToAdd"></param>
    [Theory]
    [InlineData([new[] { "a", "b", "c", "d" }, "e"])]
    [InlineData([new[] { "button", "myclass", "mx-3", "myClass", "border" }, "mx-2"])]
    public void AddsWithElementPassed(String[] elements, String elementToAdd)
    {
        var expected = elements.Append(elementToAdd).ToHashSet();
        var actual = IndexableSet.Create(elements).Add(elementToAdd);
        foreach(var element in actual)
            Assert.Contains(element, expected);
        Assert.Equal(actual.Count, expected.Count);
    }
    /// <summary>
    /// Asserts that elements passed to <see cref="IndexableSet{T}.Add(T)"/> are present in the output.
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="elementsToAdd"></param>
    [Theory]
    [InlineData([new[] { "a", "b", "c", "d" }, new[] { "e", "b", "f" }])]
    [InlineData([new[] { "button", "myclass", "mx-3", "myClass", "border" }, new[] { "mx-2", "alert", "button" }])]
    public void AddsWithElementsPassed(String[] elements, String[] elementsToAdd)
    {
        var expected = elements.Concat(elementsToAdd).ToHashSet();
        var actual = IndexableSet.Create(elements).Add(elementsToAdd);
        foreach(var element in actual)
            Assert.Contains(element, expected);
        Assert.Equal(actual.Count, expected.Count);
    }
    /// <summary>
    /// Asserts that elements passed to <see cref="IndexableSet{T}.Add(T)"/> are present in the output.
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="elementsToRemove"></param>
    [Theory]
    [InlineData([new[] { "a", "b", "c", "d" }, new[] { "c", "a", "a" }])]
    [InlineData([new[] { "button", "myclass", "mx-3", "myClass", "border" }, new[] { "button", "border", "button" }])]
    public void RemovesWithElementsPassed(String[] elements, String[] elementsToRemove)
    {
        var expected = elements.Except(elementsToRemove).ToHashSet();
        var actual = IndexableSet.Create(elements).Remove(elementsToRemove);
        foreach(var element in actual)
            Assert.Contains(element, expected);
        Assert.Equal(actual.Count, expected.Count);
    }
    /// <summary>
    /// Asserts that elements passed to <see cref="IndexableSet{T}.Remove(T)(T)"/> are not present in the output.
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="elementToRemove"></param>
    [Theory]
    [InlineData([new[] { "a", "b", "c", "d" }, "b"])]
    [InlineData([new[] { "a", "b", "c", "d" }, "e"])]
    [InlineData([new[] { "button", "myclass", "mx-3", "myClass", "border" }, "mx-3"])]
    public void RemovesWithElementPassed(String[] elements, String elementToRemove)
    {
        var actual = IndexableSet.Create(elements).Remove(elementToRemove);
        Assert.DoesNotContain(elementToRemove, actual);
    }
    /// <summary>
    /// Asserts that elements passed to <see cref="IndexableSet{T}.Add(T)"/> are present in the output and distinct.
    /// </summary>
    /// <param name="elements"></param>
    /// <param name="elementToAdd"></param>
    [Theory]
    [InlineData([new[] { "a", "b", "b", "c", "d", "a" }, "c"])]
    [InlineData([new[] { "button", "border", "border", "myclass", "mx-3", "myClass", "border" }, "mx-3"])]
    public void AddsWithDistinctElementsPassed(String[] elements, String elementToAdd)
    {
        var expected = elements.Append(elementToAdd).ToHashSet();
        var actual = IndexableSet.Create(elements).Add(elementToAdd);
        foreach(var element in actual)
            Assert.Contains(element, expected);
        Assert.Equal(actual.Count, expected.Count);
    }
}
