namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Registers detected services.
/// </summary>
/// <param name="registrationInfo">Information on the service types to register.</param>
/// <returns>The registrations to add to the container.</returns>
public delegate IEnumerable<ConventionalServiceRegistration> ConventionalServiceRegistrationDerivation(ConventionalServiceRegistrationInfo registrationInfo);
/// <summary>
/// Contains common <see cref="ConventionalServiceRegistrationDerivation"/> instances.
/// </summary>
public static class ConventionalServiceRegistrationDerivations
{
    /// <summary>
    /// Gets the default callback that will register the located service implementation onto the service type,
    /// as well as the traditional service adapter type onto the traditional service type.
    /// </summary>
    public static ConventionalServiceRegistrationDerivation Default { get; } = info =>
    [
        new(info.ServiceType, info.ImplementationType, Lifestyle.Scoped, false),
        new(info.TraditionalServiceType, info.TraditionalServiceAdapterType, Lifestyle.Scoped, false)
    ];
}
