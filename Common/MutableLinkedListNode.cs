namespace RhoMicro.ApplicationFramework.Common;

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Non-threadsafe naive implementation of a doubly linked list.
/// </summary>
/// <typeparam name="T">The type of value contained by nodes.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="value">The value contained in the new node.</param>
public sealed class MutableLinkedListNode<T>(T value) : IEnumerable<MutableLinkedListNode<T>>
{
    /// <summary>
    /// Options for configuring a nodes enumeration behavior.
    /// </summary>
    public enum EnumerationBehaviorOption
    {
        /// <summary>
        /// <see cref="GetEnumerator"/> will return an enumerator 
        /// that enumerates the nodes next neighbors, including 
        /// the node itself.
        /// </summary>
        Next,
        /// <summary>
        /// <see cref="GetEnumerator"/> will return an enumerator 
        /// that enumerates the nodes previous neighbors, including 
        /// the node itself.
        /// </summary>
        Previous
    }

    /// <summary>
    /// Gets or sets the nodes enumeration behavior option when calling <see cref="GetEnumerator"/>.
    /// </summary>
    public EnumerationBehaviorOption EnumerationBehavior { get; set; }
    /// <summary>
    /// Gets or sets the nodes current value.
    /// </summary>
    public T Value { get; set; } = value;
    /// <summary>
    /// Gets or sets the nodes next neighbor.
    /// </summary>
    public MutableLinkedListNode<T>? Next { get; set; }
    /// <summary>
    /// Gets or sets the nodes previous neighbor.
    /// </summary>
    public MutableLinkedListNode<T>? Previous { get; set; }
    /// <summary>
    /// Enumerates over the nodes next neighbors. The node itself will be included in the enumeration.
    /// </summary>
    /// <returns>An enumeration over the nodes next neighbors.</returns>
    public IEnumerable<MutableLinkedListNode<T>> NextNodes()
    {
        var node = this;
        while(node != null)
        {
            yield return node;
            node = node.Next;
        }
    }
    /// <summary>
    /// Enumerates over the nodes previous neighbors. The node itself will be included in the enumeration.
    /// </summary>
    /// <returns>An enumeration over the nodes previous neighbors.</returns>
    public IEnumerable<MutableLinkedListNode<T>> PreviousNodes()
    {
        var node = this;
        while(node != null)
        {
            yield return node;
            node = node.Previous;
        }
    }
    /// <inheritdoc/>
    public IEnumerator<MutableLinkedListNode<T>> GetEnumerator() =>
        EnumerationBehavior == EnumerationBehaviorOption.Next ?
            NextNodes().GetEnumerator() :
            PreviousNodes().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
