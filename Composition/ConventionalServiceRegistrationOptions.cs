namespace RhoMicro.ApplicationFramework.Composition;

using System.Reflection;

/// <summary>
/// Options for informing conventional service registrations.
/// </summary>
public sealed record ConventionalServiceRegistrationOptions
{
    /// <summary>
    /// Gets the default options.
    /// </summary>
    public static ConventionalServiceRegistrationOptions Default { get; } = new();
    /// <summary>
    /// Creates options with a predicate for discovering all services, while
    /// registering with a preference of those originating from the specified
    /// assemblies.
    /// </summary>
    /// <param name="assemblies">
    /// The assemblies whose services to prioritize when registering duplicate services.
    /// </param>
    /// <returns></returns>
    public static ConventionalServiceRegistrationOptions PreferAssemblies(params Assembly[] assemblies) => new()
    {
        RegistrationPredicate = ConventionalServiceRegistrationPredicates.RegisterAll,
        RegistrationDerivation = ConventionalServiceRegistrationDerivations.PreferAssemblies(assemblies)
    };
    /// <summary>
    /// Gets or sets a predicate determining whether to register a given service
    /// implementation.
    /// </summary>
    public ConventionalServiceRegistrationPredicate RegistrationPredicate { get; init; } = ConventionalServiceRegistrationPredicates.RegisterAll;
    /// <summary>
    /// Gets or sets the callback used for deriving service registrations from
    /// discovered service types.
    /// </summary>
    public ConventionalServiceRegistrationDerivation RegistrationDerivation { get; init; } = ConventionalServiceRegistrationDerivations.Default;
}
