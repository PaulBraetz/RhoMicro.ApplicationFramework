namespace TA.Composition;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Decorators;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

using TA.Presentation.Views.Blazor;

/// <summary>
/// Contains template composers.
/// </summary>
public static class Composers
{
    private static IComposer Default { get; } = Composer.Empty;
    /// <summary>
    /// Gets the default composition root for web application servers.
    /// </summary>
    public static IComposer WebGui { get; } = Default + Composer.Create(c =>
    {
        c.RegisterServices(typeof(ToLowerService).Assembly);
        c.RegisterInstance<ITimeoutSettings<ToLower>>(TimeoutSettings.Create<ToLower>(TimeSpan.FromSeconds(2)));
    });
    private static IComposer Client { get; } = Composer.Create(c =>
    {
        c.RegisterServices(typeof(ToLowerService).Assembly, new ConventionalServiceRegistrationOptions()
        {
            RegistrationPredicate = ConventionalServiceRegistrationPredicates.RegisterAll,
            RegistrationCallback = ctx =>
            {
                if(ctx.RegistrationInfo.ServiceType != typeof(IService<ToLower, ToLower.Result>))
                {
                    ctx.Container.Register(ctx.RegistrationInfo.ServiceType, ctx.RegistrationInfo.ImplementationType, Lifestyle.Scoped);
                }
                ctx.Container.Register(ctx.RegistrationInfo.TraditionalServiceType, ctx.RegistrationInfo.TraditionalServiceAdapterType, Lifestyle.Scoped);
            }
        });
    });
    /// <summary>
    /// Gets the default composition root for web application clients.
    /// </summary>
    public static IComposer WebGuiClient { get; } = Default + Client;
    /// <summary>
    /// Gets the default composition root for local application.
    /// </summary>
    public static IComposer LocalGui { get; } = Default + Client;
}
