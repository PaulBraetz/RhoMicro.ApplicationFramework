namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Represents the registration resulting from a conventional service discovery.
/// </summary>
/// <param name="ServiceType">The type of service to register.</param>
/// <param name="ImplementationType">The type of implementation to register for <see cref="ServiceType"/>.</param>
/// <param name="Lifestyle">The lifestyle of the service to register.</param>
/// <param name="OverridePreexistingRegistration">
/// Indicates whether this registration may override another service registration that resulted from the same conventional service discovery as this one.
/// </param>
public sealed record ConventionalServiceRegistration(Type ServiceType, Type ImplementationType, Lifestyle Lifestyle, Boolean OverridePreexistingRegistration);
