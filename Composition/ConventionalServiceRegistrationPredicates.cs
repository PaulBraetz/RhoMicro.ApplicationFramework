namespace RhoMicro.ApplicationFramework.Composition;

using System.Reflection;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Determines whether a given implementation should be registered for a given service type.
/// </summary>
/// <param name="context">The service and implementation type to check.</param>
/// <returns><see langword="true"/> if the implementation type in <paramref name="context"/> should be registered; otherwise, <see langword="false"/>.</returns>
public delegate Boolean ConventionalServiceRegistrationPredicate(ConventionalServiceRegistrationContext context);
/// <summary>
/// Contains common <see cref="ConventionalServiceRegistrationPredicate"/> instances.
/// </summary>
public static class ConventionalServiceRegistrationPredicates
{
    /// <summary>
    /// Gets a predicate that does not filter any service implementations.
    /// </summary>
    public static ConventionalServiceRegistrationPredicate RegisterAll { get; } = ctx => true;
    /// <summary>
    /// Gets a predicate that filters all service implementations.
    /// </summary>
    public static ConventionalServiceRegistrationPredicate RegisterNone { get; } = ctx => false;
    /// <summary>
    /// Gets a predicate that registers all services except those implementing an api request execution.
    /// This predicate is useful for registering services to clients (as they should receive api handlers).
    /// </summary>
    public static ConventionalServiceRegistrationPredicate RegisterAlLExceptApiImplementations { get; } = 
        ctx => !ctx.ServiceType.GenericTypeArguments[0].GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IApiRequest<,,,>))
                .Any();
    /// <summary>
    /// Gets a predicate that filters all implementations that are annotated with the <see cref="FakeServiceAttribute"/>.
    /// </summary>
    public static ConventionalServiceRegistrationPredicate IgnoreAttributeFakes { get; } =
        ctx => ctx.ImplementationType.GetCustomAttribute<FakeServiceAttribute>(inherit: true) == null;
    /// <summary>
    /// Gets a predicate that filters all implementations whose name or namespace contain 'fake' (case-insensitive).
    /// </summary>
    public static ConventionalServiceRegistrationPredicate IgnoreNameFakes { get; } =
        ctx => !ctx.ImplementationType.Name.Contains("fake", StringComparison.InvariantCultureIgnoreCase) &&
                ( ctx.ImplementationType.Namespace == null || !ctx.ImplementationType.Namespace.Contains("fake", StringComparison.InvariantCultureIgnoreCase) );
    /// <summary>
    /// Gets a predicate that filters all implementations whose name or namespace contain 'fake' (case-insensitive) or that are annotated with the <see cref="FakeServiceAttribute"/>.
    /// </summary>
    public static ConventionalServiceRegistrationPredicate IgnoreNameAndAttributeFakes { get; } =
        ctx => IgnoreAttributeFakes.Invoke(ctx) && IgnoreNameFakes.Invoke(ctx);
}