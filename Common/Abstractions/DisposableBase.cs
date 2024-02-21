namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Base class for types implementing <see cref="IDisposable"/>.
/// Instances may only be disposed once. Concurrent calls to <see cref="Dispose()"/> will
/// be ignored; only one call may enter the method. Exceptions thrown during disposal will cause
/// the instance state to revert to 'not-disposed'.
/// </summary>
public abstract class DisposableBase : IDisposable
{
    private const Int32 _disposedState = 1;
    private const Int32 _nonDisposedState = 0;
    private const Int32 _disposingState = -1;

    private Int32 _disposedValue = _nonDisposedState;

    /// <summary>
    /// Disposes the instance.
    /// </summary>
    /// <param name="disposing">Should be set to <see langword="true"/> if disposing from a <see cref="Dispose()"/> call; otherwise, <see langword="false"/> (occurs during finalization).</param>
#pragma warning disable CA1063 // Implement IDisposable Correctly <- case of mistaken identity
    private void Dispose(Boolean disposing)
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        if(Interlocked.CompareExchange(ref _disposedValue, _disposingState, _nonDisposedState) != _nonDisposedState)
            return;

        try
        {
            if(disposing)
                DisposeManaged();

            DisposeUnmanaged();
            _disposedValue = _disposedState;
        } catch
        {
            _disposedValue = _nonDisposedState;
            throw;
        }
    }
    /// <summary>
    /// Disposes of managed state.
    /// </summary>
    protected virtual void DisposeManaged()
    {

    }
    /// <summary>
    /// Disposes unmanaged state and sets large fields to null. 
    /// When overriding this method, make sure to also implement a finalizer that calls <see cref="DisposeFinalize"/>.
    /// </summary>
    protected virtual void DisposeUnmanaged() { }
    /// <summary>
    /// This method should only be invoked in the finalizer of classes that also override <see cref="DisposeUnmanaged"/>.
    /// </summary>
    protected void DisposeFinalize() => Dispose(false);

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(Boolean disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
