namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Options for informing conventional service registrations.
/// </summary>
public sealed class ConventionalServiceRegistrationOptions
{
    internal static ConventionalServiceRegistrationOptions Default { get; } = new();
    /// <summary>
    /// Gets or sets a value determining the behavior upon encountering duplicate service implementations.
    /// If set to <see langword="true"/>, duplicate implementations will be ignored; otherwise, an exception will be thrown.
    /// </summary>
    public Boolean IgnoreDuplicates { get; set; } = true;
    /// <summary>
    /// Gets or sets a predicate determining whether to register a given service implementation.
    /// </summary>
    public ConventionalServiceRegistrationPredicate RegistrationPredicate { get; set; } = ConventionalServiceRegistrationPredicates.RegisterAll;
    /// <summary>
    /// Gets or sets the callback used for actually registering services to the container.
    /// </summary>
    public ConventionalServiceRegistrationCallback RegistrationCallback { get; set; } = ConventionalServiceRegistrationCallbacks.Default;
}
