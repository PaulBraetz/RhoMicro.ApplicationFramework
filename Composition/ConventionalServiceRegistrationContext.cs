namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Provides detected service and implementation types, as well as the container to optionally register them to.
/// </summary>
/// <param name="RegistrationInfo">Information on the service types to register.</param>
/// <param name="Container">The container to register services to.</param>
public sealed record ConventionalServiceRegistrationCallbackContext(ConventionalServiceRegistrationInfo RegistrationInfo, Container Container);

