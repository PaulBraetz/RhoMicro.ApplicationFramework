namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Options for informing conventional service registrations.
/// </summary>
public sealed class ConventionalServiceRegistrationOptions
{
    internal static ConventionalServiceRegistrationOptions Default { get; } = new();
    /// <summary>
    /// Gets or sets a callback invoked to determine the lifestyle of registered service implementations.
    /// </summary>
    public Func<ConventionalServiceRegistrationContext, Lifestyle> LifestyleFactory { get; set; } = ctx => Lifestyle.Scoped;
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
    /// Gets or sets a projection determining the actual implementation type to register given a service type.
    /// </summary>
    public ConventionalServiceRegistrationProjection RegistrationProjection { get; set; } = ConventionalServiceRegistrationProjections.Default;
}
