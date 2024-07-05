namespace RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Provides detected service and implementation types.
/// </summary>
/// <param name="ServiceType">The type of service to register.</param>
/// <param name="ImplementationType">The type of implementation to register for the service.</param>
/// <param name="TraditionalServiceType">The traditional service interface to register.</param>
/// <param name="TraditionalServiceAdapterType">The adapter adapting the service type onto the traditional service type.</param>
public sealed record ConventionalServiceRegistrationInfo(Type ServiceType, Type ImplementationType, Type TraditionalServiceType, Type TraditionalServiceAdapterType);

