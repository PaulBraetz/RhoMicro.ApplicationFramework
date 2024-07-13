namespace RhoMicro.ApplicationFramework.Composition;
/// <summary>
/// Options for informing conventional service registrations.
/// </summary>
public sealed class ConventionalServiceRegistrationOptions
{
    internal static ConventionalServiceRegistrationOptions Default { get; } = new();
    /// <summary>
    /// Gets or sets a predicate determining whether to register a given service implementation.
    /// </summary>
    public ConventionalServiceRegistrationPredicate RegistrationPredicate { get; set; } = ConventionalServiceRegistrationPredicates.RegisterAll;
    /// <summary>
    /// Gets or sets the callback used for actually registering services to the container.
    /// </summary>
    public ConventionalServiceRegistrationCallback RegistrationCallback { get; set; } = ConventionalServiceRegistrationCallbacks.Default;
}
