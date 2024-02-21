namespace RhoMicro.ApplicationFramework.Common.Tests;
#pragma warning disable CA1861 // Avoid constant arrays as arguments
using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Contains tests for <see cref="AmbientCallbackStateQueue{T}"/>.
/// </summary>
[TestClass]
public class AmbientCallbackStateQueueTests
{
    /// <summary>
    /// Asserts that a scope returned from <see cref="AmbientCallbackStateQueue{T}.EnqueueLateConditional(Func{Boolean}, Func{T}, Action{IEnumerable{T}})"/> may not be disposed of twice.
    /// </summary>
    /// <param name="value">The value to enqueue.</param>
    [TestMethod]
    [DataRow(6)]
    [DataRow(-1)]
    [DataRow(Int32.MaxValue)]
    [DataRow(Int32.MinValue)]
    [DataRow(-8)]
    public void ThrowsODEUponSecondLateConditionalScopeDispose(Int32 value)
    {
        //Arrange
        var scope = AmbientCallbackStateQueue<Int32>.EnqueueLateConditional(() => true, () => value, _ => { });

        //Act
        scope.Dispose();

        //Assert
        _ = Assert.ThrowsException<ObjectDisposedException>(scope.Dispose);
    }
    /// <summary>
    /// Asserts that a scope returned from <see cref="AmbientCallbackStateQueue{T}.EnqueueLate(Func{T}, Action{IEnumerable{T}})"/> may not be disposed of twice.
    /// </summary>
    /// <param name="value">The value to enqueue.</param>
    [TestMethod]
    [DataRow(6)]
    [DataRow(-1)]
    [DataRow(Int32.MaxValue)]
    [DataRow(Int32.MinValue)]
    [DataRow(-8)]
    public void ThrowsODEUponSecondLateScopeDispose(Int32 value)
    {
        //Arrange
        var scope = AmbientCallbackStateQueue<Int32>.EnqueueLate(() => value, _ => { });

        //Act
        scope.Dispose();

        //Assert
        _ = Assert.ThrowsException<ObjectDisposedException>(scope.Dispose);
    }
    /// <summary>
    /// Asserts that a scope returned from <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/> may not be disposed of twice.
    /// </summary>
    /// <param name="value">The value to enqueue.</param>
    [TestMethod]
    [DataRow(6)]
    [DataRow(-1)]
    [DataRow(Int32.MaxValue)]
    [DataRow(Int32.MinValue)]
    [DataRow(-8)]
    public void ThrowsODEUponSecondScopeDispose(Int32 value)
    {
        //Arrange
        var scope = AmbientCallbackStateQueue<Int32>.Enqueue(value, _ => { });

        //Act
        scope.Dispose();

        //Assert
        _ = Assert.ThrowsException<ObjectDisposedException>(scope.Dispose);
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/> will enqueue values in order of calls.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/>.</param>
    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [DataRow(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [DataRow(new[] { 4, 5, 7, 8, 0 })]
    public void QueueIsClearedAfterCallback(Int32[] values)
    {
        ArgumentNullException.ThrowIfNull(values);

        //Arrange
        var actualFirst = new List<Int32>();
        var expectedFirst = values.Take(values.Length / 2).ToArray();
        IDisposable enqueueFirst(Int32 v) => AmbientCallbackStateQueue<Int32>.Enqueue(v, actualFirst!.AddRange);

        var actualSecond = new List<Int32>();
        var expectedSecond = values.Skip(values.Length / 2).ToArray();
        IDisposable enqueueSecond(Int32 v) => AmbientCallbackStateQueue<Int32>.Enqueue(v, actualSecond!.AddRange);

        //Act
        var firstScopes = expectedFirst.Select(enqueueFirst).Reverse().ToArray();
        foreach(var scope in firstScopes)
        {
            scope.Dispose();
        }

        var secondScopes = expectedSecond.Select(enqueueSecond).Reverse().ToArray();
        foreach(var scope in secondScopes)
        {
            scope.Dispose();
        }

        //Assert
        Assert.IsTrue(actualFirst.SequenceEqual(expectedFirst));
        Assert.IsTrue(actualSecond.SequenceEqual(expectedSecond));
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/> will enqueue values in order of calls.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/>.</param>
    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [DataRow(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [DataRow(new[] { 4, 5, 7, 8, 0 })]
    public void SynchronousNestedScopesEnqueueInOrder(Int32[] values)
    {
        //Arrange
        var actual = new List<Int32>();
        IDisposable enqueue(Int32 v) => AmbientCallbackStateQueue<Int32>.Enqueue(v, actual.AddRange);
        var expected = values;

        //Act
        var scopes = values.Select(enqueue).Reverse().ToArray();
        foreach(var scope in scopes)
        {
            scope.Dispose();
        }

        //Assert
        Assert.IsTrue(actual.SequenceEqual(expected));
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.EnqueueLate(Func{T}, Action{IEnumerable{T}})"/> will enqueue values in reverse order of calls.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.EnqueueLate(Func{T}, Action{IEnumerable{T}})"/>.</param>
    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [DataRow(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [DataRow(new[] { 4, 5, 7, 8, 0 })]
    public void SynchronousNestedScopesEnqueueLateInReverseOrder(Int32[] values)
    {
        //Arrange
        var actual = new List<Int32>();
        IDisposable enqueue(Int32 v) =>
            AmbientCallbackStateQueue<Int32>.EnqueueLate(() => v, actual.AddRange);
        var expected = values.Reverse().ToArray();

        //Act
        var scopes = values.Select(enqueue).Reverse().ToArray();
        foreach(var scope in scopes)
        {
            scope.Dispose();
        }

        //Assert
        Assert.IsTrue(actual.SequenceEqual(expected));
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.EnqueueLateConditional(Func{Boolean}, Func{T}, Action{IEnumerable{T}})"/> will enqueue values in reverse order of calls and respect the confirmation func.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.EnqueueLateConditional(Func{Boolean}, Func{T}, Action{IEnumerable{T}})"/>.</param>
    [TestMethod]
    [DataRow(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [DataRow(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [DataRow(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [DataRow(new[] { 4, 5, 7, 8, 0 })]
    public void SynchronousNestedScopesEnqueueLateConditionalInReverseOrder(Int32[] values)
    {
        //Arrange
        var actual = new List<Int32>();
        Boolean filter(Int32 value) =>
            value % 2 == 0;
        IDisposable enqueue(Int32 v) =>
            AmbientCallbackStateQueue<Int32>.EnqueueLateConditional(() => filter(v), () => v, actual.AddRange);
        var expected = values.Where(filter).Reverse().ToArray();

        //Act
        var scopes = values.Select(enqueue).Reverse().ToArray();
        foreach(var scope in scopes)
        {
            scope.Dispose();
        }

        //Assert
        Assert.IsTrue(actual.SequenceEqual(expected));
    }
}
