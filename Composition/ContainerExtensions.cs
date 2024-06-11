namespace RhoMicro.ApplicationFramework.Composition;

using RhoMicro.ApplicationFramework.Common.Abstractions;

using SimpleInjector;

/// <summary>
/// Contains extensions for <see cref="Container"/>.
/// </summary>
public static class ContainerExtensions
{
    /// <summary>
    /// Registers a service to the container (transient).
    /// </summary>
    /// <typeparam name="TService">The type of service to register.</typeparam>
    /// <typeparam name="TRequest">The type of request processed by the service.</typeparam>
    /// <typeparam name="TResult">The type of result produced by the service.</typeparam>
    /// <param name="container">The container to register the service to.</param>
    public static void RegisterService<TService, TRequest, TResult>(this Container container)
        where TService : class, IService<TRequest, TResult>
        where TRequest : IServiceRequest<TResult> => container.RegisterService<TService, TRequest, TResult>(Lifestyle.Transient);
    /// <summary>
    /// Registers a service to the container.
    /// </summary>
    /// <typeparam name="TService">The type of service to register.</typeparam>
    /// <typeparam name="TRequest">The type of request processed by the service.</typeparam>
    /// <typeparam name="TResult">The type of result produced by the service.</typeparam>
    /// <param name="container">The container to register the service to.</param>
    /// <param name="lifestyle">The lifestyle to register the service with.</param>
    public static void RegisterService<TService, TRequest, TResult>(this Container container, Lifestyle lifestyle)
        where TService : class, IService<TRequest, TResult>
        where TRequest : IServiceRequest<TResult>
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register<IService<TRequest, TResult>, TService>(lifestyle);
    }
}