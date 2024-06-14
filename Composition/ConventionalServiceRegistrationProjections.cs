namespace RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Determines the implementation type to use given a service and implementation type should be registered for a given service type.
/// </summary>
/// <param name="context">The service and implementation type to project.</param>
/// <returns>The implementation type to use for the service provided.</returns>
public delegate Type ConventionalServiceRegistrationProjection(ConventionalServiceRegistrationContext context);
/// <summary>
/// Contains common <see cref="ConventionalServiceRegistrationProjection"/> instances.
/// </summary>
public static class ConventionalServiceRegistrationProjections
{
    /// <summary>
    /// Gets the default projection that will return <see cref="ConventionalServiceRegistrationContext.ImplementationType"/>.
    /// </summary>
    public static ConventionalServiceRegistrationProjection Default { get; } = ctx => ctx.ImplementationType;
}
