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
    /// assembly.
    /// </summary>
    /// <param name="assembly">
    /// The assembly to prefer registering services from.
    /// </param>
    /// <returns></returns>
    public static ConventionalServiceRegistrationOptions PreferAssembly(Assembly assembly) => new()
    {
        RegistrationPredicate = ConventionalServiceRegistrationPredicates.RegisterAll,
        RegistrationDerivation = ConventionalServiceRegistrationDerivations.PreferAssembly(assembly)
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
