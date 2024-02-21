namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Strategy-based implementation of <see cref="IFactory{TProduct}"/>.
/// </summary>
/// <typeparam name="TProduct">The type of object to create.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="strategy">The strategy to invoke when calling <see cref="Create"/>.</param>
public sealed class FactoryStrategy<TProduct>(Func<TProduct> strategy) : IFactory<TProduct>
{
    /// <inheritdoc/>
    public TProduct Create() => strategy.Invoke();
}

/// <summary>
/// Strategy-based implementation of <see cref="IFactory{TProduct, TParams}"/>.
/// </summary>
/// <typeparam name="TProduct">The type of object to create.</typeparam>
/// <typeparam name="TParams">The type of parameters required to create a new instance.</typeparam>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="strategy">The strategy to use when calling <see cref="Create(TParams)"/>.</param>
public sealed class FactoryStrategy<TProduct, TParams>(Func<TParams, TProduct> strategy) : IFactory<TProduct, TParams>
{
    /// <inheritdoc/>
    public TProduct Create(TParams parameters) => strategy.Invoke(parameters);
}