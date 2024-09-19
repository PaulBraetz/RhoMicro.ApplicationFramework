namespace RhoMicro.ApplicationFramework.Composition;

using System.Reflection;

using SimpleInjector;

/// <summary>
/// Registers detected services.
/// </summary>
/// <param name="registrationInfo">Information on the service types to register.</param>
/// <returns>The registrations to add to the container.</returns>
public delegate IEnumerable<ConventionalServiceRegistration> ConventionalServiceRegistrationDerivation(ConventionalServiceRegistrationInfo registrationInfo);
/// <summary>
/// Contains common <see cref="ConventionalServiceRegistrationDerivation"/>
/// instances.
/// </summary>
public static class ConventionalServiceRegistrationDerivations
{
    /// <summary>
    /// Gets the default callback that will register the located service
    /// implementation onto the service type, as well as the traditional service
    /// adapter type onto the traditional service type.
    /// </summary>
    public static ConventionalServiceRegistrationDerivation Default { get; } = info =>
    [
        new(info.ServiceType, info.ImplementationType, Lifestyle.Scoped, false),
        new(info.TraditionalServiceType, info.TraditionalServiceAdapterType, Lifestyle.Scoped, false)
    ];
    /// <summary>
    /// Creates a callback that will prefer registering services from the
    /// specified assemblies. Providing duplicate service implementations from
    /// the assemblies provided will likely lead to priority conflicts.
    /// </summary>
    /// <param name="assemblies">
    /// The assemblies whose services to prioritize when registering duplicate services.
    /// </param>
    /// <returns></returns>
    public static ConventionalServiceRegistrationDerivation PreferAssemblies(params Assembly[] assemblies)
    {
        var assemblySet = assemblies.ToHashSet();
        return info =>
        {
            var isOverride = assemblySet.Contains(info.ImplementationType.Assembly);
            return [
                new(info.ServiceType,info.ImplementationType, Lifestyle.Scoped, isOverride),
                new(info.TraditionalServiceType, info.TraditionalServiceAdapterType, Lifestyle.Scoped, isOverride)
            ];
        };
    }
}
