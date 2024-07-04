namespace RhoMicro.ApplicationFramework.Composition;

using System.Reflection;

using RhoMicro.ApplicationFramework.Common;
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
        where TRequest : IRequest<TResult> => container.RegisterService<TService, TRequest, TResult>(Lifestyle.Transient);
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
        where TRequest : IRequest<TResult>
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register<IService<TRequest, TResult>, TService>(lifestyle);
    }
    /// <summary>
    /// Conventionally registers all generated implementations of <see cref="IService{TRequest, TResult}"/> to the container.
    /// </summary>
    /// <param name="container">The container to register services to.</param>
    /// <param name="assembly">The assembly to query for generated implementations of <see cref="IService{TRequest, TResult}"/>.</param>
    /// <param name="options">The optional options to use when registering services.</param>
    public static void RegisterServices(this Container container, Assembly assembly, ConventionalServiceRegistrationOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(assembly);

        options ??= ConventionalServiceRegistrationOptions.Default;

        var registrations = assembly.GetTypes()
            .Select(t => t.GetCustomAttribute<ServiceInjectionInfoAttribute>())
            .Where(a => a != null)
            .Select(a =>
            {
                var aopServiceType = typeof(IService<,>).MakeGenericType(a!.RequestType, a.ResultType);

                var value = new
                {
                    TraditionalServiceType = a.ServiceType,
                    TraditionalImplementationType = a.AdapterType,
                    ImplementationTypes = new List<Type>() { a.ImplementationType }
                };

                return (aopServiceType, value);
            }).ToDictionary(t => t.aopServiceType, t => t.value);

        foreach(var (serviceType, data) in registrations)
        {
            if(data.ImplementationTypes is not [{ } implementationType])
            {
                if(options.IgnoreDuplicates)
                    continue;

                throw new ConventionalServiceRegistrationDuplicateException(serviceType, data.ImplementationTypes);
            }

            var context = new ConventionalServiceRegistrationContext(ServiceType: serviceType, ImplementationType: implementationType);

            var lifestyle = options.LifestyleFactory.Invoke(context);

            if(options.RegistrationPredicate.Invoke(context))
            {
                var actualImplementationType = options.RegistrationProjection.Invoke(context);
                container.Register(serviceType, actualImplementationType, lifestyle);
            }

            container.Register(data.TraditionalServiceType, data.TraditionalImplementationType, lifestyle);
        }
    }
}
