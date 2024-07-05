namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

/// <summary>
/// Registers detected services.
/// </summary>
/// <param name="context">The service and implementation type to project.</param>
/// <returns>The implementation type to use for the service provided.</returns>
public delegate void ConventionalServiceRegistrationCallback(ConventionalServiceRegistrationCallbackContext context);
/// <summary>
/// Contains common <see cref="ConventionalServiceRegistrationCallback"/> instances.
/// </summary>
public static class ConventionalServiceRegistrationCallbacks
{
    /// <summary>
    /// Gets the default callback that will register the located service implementation onto the service type,
    /// as well as the traditional service adapter type onto the traditional service type.
    /// </summary>
    public static ConventionalServiceRegistrationCallback Default { get; } = ctx =>
    {
        ctx.Container.Register(ctx.RegistrationInfo.ServiceType, ctx.RegistrationInfo.ImplementationType, Lifestyle.Scoped);
        ctx.Container.Register(ctx.RegistrationInfo.TraditionalServiceType, ctx.RegistrationInfo.TraditionalServiceAdapterType, Lifestyle.Scoped);
    };
}
