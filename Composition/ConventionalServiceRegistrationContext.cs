namespace RhoMicro.ApplicationFramework.Composition;
/// <summary>
/// Communicates the service and implementation type for which to determine a lifestyle.
/// </summary>
/// <param name="ServiceType">The type of service to register.</param>
/// <param name="ImplementationType">The type of implementation to register for the service.</param>
public readonly record struct ConventionalServiceRegistrationContext(Type ServiceType, Type ImplementationType);

