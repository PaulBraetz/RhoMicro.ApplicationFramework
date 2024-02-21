namespace RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Abstract factory for creating instances of <typeparamref name="TProduct"/>.
/// </summary>
/// <typeparam name="TProduct">The type of object to create.</typeparam>
public interface IFactory<TProduct>
{
    /// <summary>
    /// Creates a new instance of <typeparamref name="TProduct"/>.
    /// </summary>
    /// <returns>A new instance of <typeparamref name="TProduct"/>.</returns>
    TProduct Create();
}

/// <summary>
/// Abstract factory for creating instances of <typeparamref name="TProduct"/>.
/// </summary>
/// <typeparam name="TProduct">The type of object to create.</typeparam>
/// <typeparam name="TParams">The type of parameters required to create a new instance.</typeparam>
public interface IFactory<TProduct, TParams>
{
    /// <summary>
    /// Creates a new instance of <typeparamref name="TProduct"/>.
    /// </summary>
    /// <param name="parameters">The parameters required to create a new instance.</param>
    /// <returns>A new instance of <typeparamref name="TProduct"/>.</returns>
    TProduct Create(TParams parameters);
}