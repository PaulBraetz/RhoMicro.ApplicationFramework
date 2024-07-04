namespace TA.Composition;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Decorators;
using RhoMicro.ApplicationFramework.Composition;

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
    /// <summary>
    /// Gets the default composition root for web application clients.
    /// </summary>
    public static IComposer WebGuiClient { get; } = Default + Composer.Create(c =>
    {
        c.RegisterServices(typeof(ToLowerService).Assembly, new ConventionalServiceRegistrationOptions()
        {
            RegistrationPredicate = ConventionalServiceRegistrationPredicates.RegisterNone
        });
    });
    /// <summary>
    /// Gets the default composition root for local application.
    /// </summary>
    public static IComposer LocalGui { get; } = Default + Composer.Create(c =>
    {
        c.RegisterServices(typeof(ToLowerService).Assembly, new ConventionalServiceRegistrationOptions()
        {
            RegistrationPredicate = ConventionalServiceRegistrationPredicates.RegisterNone
        });
    });
}
