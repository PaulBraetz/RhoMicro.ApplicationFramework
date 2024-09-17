namespace RhoMicro.ApplicationFramework.Composition;

using System.Reflection;
using System.Text.RegularExpressions;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

using SimpleInjector;

/// <summary>
/// Contains extensions for <see cref="Container"/>.
/// </summary>
public static partial class ContainerExtensions
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
    /// <param name="assemblies">The assemblies to query for generated implementations of <see cref="IService{TRequest, TResult}"/>.</param>
    public static void RegisterServices(this Container container, params Assembly[] assemblies) =>
        container.RegisterServices(ConventionalServiceRegistrationOptions.Default, assemblies);
    /// <summary>
    /// Conventionally registers all generated implementations of <see cref="IService{TRequest, TResult}"/> to the container.
    /// </summary>
    /// <param name="container">The container to register services to.</param>
    /// <param name="options">The optional options to use when registering services.</param>
    /// <param name="assemblies">The assemblies to query for generated implementations of <see cref="IService{TRequest, TResult}"/>.</param>
    public static void RegisterServices(this Container container, ConventionalServiceRegistrationOptions options, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(assemblies);
        ArgumentNullException.ThrowIfNull(options);

        var registrations = assemblies.SelectMany(a => a.GetTypes())
            .SelectMany(t => t.GetCustomAttributes<ServiceInjectionInfoAttribute>())
            .Where(a => a != null)
            .Select(a =>
                new ConventionalServiceRegistrationInfo(
                    ServiceType: typeof(IService<,>).MakeGenericType(a!.RequestType, a.ResultType),
                    ImplementationType: a.ImplementationType,
                    TraditionalServiceType: a.ServiceType,
                    TraditionalServiceAdapterType: a.AdapterType))
            .Where(options.RegistrationPredicate.Invoke)
            .SelectMany(options.RegistrationDerivation.Invoke);

        var registrationMap = new Dictionary<Type, ConventionalServiceRegistration>();
        foreach(var registration in registrations)
        {
            var containsPrevious = registrationMap.TryGetValue(registration.ServiceType, out var previous);
            if(containsPrevious && registration.OverridePreexistingRegistration && previous != registration)
            {
                if(previous!.OverridePreexistingRegistration)
                    throw new InvalidOperationException($"Unable to handle conventional service registrations for service '{registration.ServiceType.FullName}': multiple conventional registrations ('{registration.ImplementationType.FullName}' and '{previous.ImplementationType.FullName}') are marked as '{nameof(ConventionalServiceRegistration.OverridePreexistingRegistration)}'.");

                registrationMap[registration.ServiceType] = registration;
            } else if(!containsPrevious)
            {
                registrationMap.Add(registration.ServiceType, registration);
            }
        }

        foreach(var (_, registration) in registrationMap)
        {
            try
            {
                container.Register(registration.ServiceType, registration.ImplementationType, registration.Lifestyle);
            } catch(InvalidOperationException ex)
            when(GetDuplicateRegistrationMessagePattern().IsMatch(ex.Message))
            {
                throw new InvalidOperationException($"A duplicate registration for '{registration.ServiceType.FullName}' has been detected; duplicate registrations may have their precedence defined via '{nameof(options)}.{nameof(options.RegistrationDerivation)}'.", ex);
            }
        }
    }
    [GeneratedRegex(@"Type .* has already been registered\.")]
    private static partial Regex GetDuplicateRegistrationMessagePattern();
}
