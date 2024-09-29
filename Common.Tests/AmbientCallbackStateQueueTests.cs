namespace RhoMicro.ApplicationFramework.Common.Tests;

using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Contains tests for <see cref="AmbientCallbackStateQueue{T}"/>.
/// </summary>
public class AmbientCallbackStateQueueTests
{
    /// <summary>
    /// Asserts that a scope returned from <see cref="AmbientCallbackStateQueue{T}.EnqueueLateConditional(Func{Boolean}, Func{T}, Action{IEnumerable{T}})"/> may not be disposed of twice.
    /// </summary>
    /// <param name="value">The value to enqueue.</param>
    [Theory]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(Int32.MaxValue)]
    [InlineData(Int32.MinValue)]
    [InlineData(-8)]
    public void ThrowsODEUponSecondLateConditionalScopeDispose(Int32 value)
    {
        //Arrange
        var scope = AmbientCallbackStateQueue<Int32>.EnqueueLateConditional(() => true, () => value, _ => { });

        //Act
        scope.Dispose();

        //Assert
        _ = Assert.Throws<ObjectDisposedException>(scope.Dispose);
    }
    /// <summary>
    /// Asserts that a scope returned from <see cref="AmbientCallbackStateQueue{T}.EnqueueLate(Func{T}, Action{IEnumerable{T}})"/> may not be disposed of twice.
    /// </summary>
    /// <param name="value">The value to enqueue.</param>
    [Theory]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(Int32.MaxValue)]
    [InlineData(Int32.MinValue)]
    [InlineData(-8)]
    public void ThrowsODEUponSecondLateScopeDispose(Int32 value)
    {
        //Arrange
        var scope = AmbientCallbackStateQueue<Int32>.EnqueueLate(() => value, _ => { });

        //Act
        scope.Dispose();

        //Assert
        _ = Assert.Throws<ObjectDisposedException>(scope.Dispose);
    }
    /// <summary>
    /// Asserts that a scope returned from <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/> may not be disposed of twice.
    /// </summary>
    /// <param name="value">The value to enqueue.</param>
    [Theory]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(Int32.MaxValue)]
    [InlineData(Int32.MinValue)]
    [InlineData(-8)]
    public void ThrowsODEUponSecondScopeDispose(Int32 value)
    {
        //Arrange
        var scope = AmbientCallbackStateQueue<Int32>.Enqueue(value, _ => { });

        //Act
        scope.Dispose();

        //Assert
        _ = Assert.Throws<ObjectDisposedException>(scope.Dispose);
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/> will enqueue values in order of calls.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/>.</param>
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [InlineData(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [InlineData(new[] { 4, 5, 7, 8, 0 })]
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
        Assert.True(actualFirst.SequenceEqual(expectedFirst));
        Assert.True(actualSecond.SequenceEqual(expectedSecond));
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/> will enqueue values in order of calls.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.Enqueue(T, Action{IEnumerable{T}})"/>.</param>
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [InlineData(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [InlineData(new[] { 4, 5, 7, 8, 0 })]
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
        Assert.True(actual.SequenceEqual(expected));
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.EnqueueLate(Func{T}, Action{IEnumerable{T}})"/> will enqueue values in reverse order of calls.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.EnqueueLate(Func{T}, Action{IEnumerable{T}})"/>.</param>
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [InlineData(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [InlineData(new[] { 4, 5, 7, 8, 0 })]
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
        Assert.True(actual.SequenceEqual(expected));
    }
    /// <summary>
    /// Asserts that calls to <see cref="AmbientCallbackStateQueue{T}.EnqueueLateConditional(Func{Boolean}, Func{T}, Action{IEnumerable{T}})"/> will enqueue values in reverse order of calls and respect the confirmation func.
    /// </summary>
    /// <param name="values">The values to sequentially pass to <see cref="AmbientCallbackStateQueue{T}.EnqueueLateConditional(Func{Boolean}, Func{T}, Action{IEnumerable{T}})"/>.</param>
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 2, 3, 7, 8, 9, 0 })]
    [InlineData(new[] { 3, 4, Int32.MaxValue, 6, 7, 8, 9, 0 })]
    [InlineData(new[] { 1, 4, 5, 6, 7, Int32.MinValue })]
    [InlineData(new[] { 4, 5, 7, 8, 0 })]
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
        Assert.True(actual.SequenceEqual(expected));
    }
}
