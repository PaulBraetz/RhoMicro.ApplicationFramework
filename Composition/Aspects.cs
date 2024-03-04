namespace RhoMicro.ApplicationFramework.Composition;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Decorators;
using RhoMicro.ApplicationFramework.Aspects.Services;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Formatters;

using SimpleInjector;

/// <summary>
/// Contains object graph definitions for common object graphs.
/// </summary>
public static class Common
{
    /// <summary>
    /// Gets a composer able to compose common object graphs.
    /// </summary>
    public static IComposer Default { get; } = Composer.Create(c => c.RegisterInstance<IAspEnvironment>(new AspEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development")));
    /// <summary>
    /// Gets a composer able to compose common debug object graphs.
    /// </summary>
    public static IComposer Debug { get; } = Composer.Create(c => c.RegisterInstance<IAspEnvironment>(AspEnvironment.Development));

    /// <summary>
    /// Gets a composer able to compose common debug object graphs.
    /// </summary>
    public static IComposer Release { get; } = Composer.Create(c => c.RegisterInstance<IAspEnvironment>(AspEnvironment.Production));
}

/// <summary>
/// Contains object graph definitions for cross-cutting concerns.
/// </summary>
public static class Aspects
{
#pragma warning disable IDE0053 // Use expression body for lambda expression
#pragma warning disable IDE0200 // Remove unnecessary lambda expression
    /// <summary>
    /// Gets a composer able to compose common aspects.
    /// </summary>
    public static IComposer Composer { get; } =
        Composition.Composer.Create(c =>
        {
            RegisterCoreAspects(c);
        });
#pragma warning restore IDE0200 // Remove unnecessary lambda expression
#pragma warning restore IDE0053 // Use expression body for lambda expression

    private static void RegisterCoreAspects(Container c)
    {
        RegisterFormatters(c);
        RegisterLoggingService(c);

        RegisterServiceCancellationDecorators(c);
        //Consider enabling this in contexts where the Ui sync context only allows for a single thread.
        //RegisterSyncContextInterceptionProxies(c);
        RegisterLoggingDecorators(c);
    }
    private static void RegisterServiceCancellationDecorators(Container c) =>
        c.RegisterDecorator(typeof(IService<>), typeof(ServiceCancellationDecorator<>), Lifestyle.Scoped);

    private static void RegisterLoggingDecorators(Container c)
    {
        //timestamp->execution->thread->exception->time->service

        c.RegisterDecorator(typeof(IService<,>), typeof(ExecutionTimeLoggingServiceDecorator<,>), Lifestyle.Scoped);
        c.RegisterDecorator(typeof(IService<,>), typeof(ExceptionLoggingServiceDecorator<,>), Lifestyle.Scoped);
        c.RegisterDecorator(typeof(IService<,>), typeof(ThreadIdLoggingServiceDecorator<,>), Lifestyle.Scoped);
        c.RegisterDecorator(typeof(IService<,>), typeof(ExecutionLoggingServiceDecorator<,>), Lifestyle.Scoped);
        c.RegisterDecorator(typeof(IService<,>), ctx =>
        {
            var implementationType = ctx.ImplementationType;

            /*
            Not required for now, as proxied service is decorated also.
            Thus, both the proxy service as well as the proxied service will be logged in full.
            if(implementationType.IsConstructedGenericType)
            {
                var typeDef = implementationType.GetGenericTypeDefinition();
                if(typeDef == typeof(ProxyService<,,>))
                {
                    //replace impl type using proxied service type
                    implementationType = implementationType.GenericTypeArguments[2];
                }
            }
            */

            var serviceArgs = ctx.ServiceType.GenericTypeArguments;
            var result = typeof(ServiceTypeLoggingDecorator<,,>)
                .MakeGenericType(serviceArgs[0], serviceArgs[1], implementationType);

            return result;
        }, Lifestyle.Scoped, ctx => true);
    }

    private static void RegisterLoggingService(Container c)
    {
        c.Register(() => new LoggingService(c.GetRequiredService<ILoggerFactory>().CreateLogger<LoggingService>()), Lifestyle.Scoped);
        c.Register<ILoggingService>(c.GetRequiredService<LoggingService>, Lifestyle.Scoped);
    }

    private static void RegisterFormatters(Container c)
    {
        c.Register<EllipsisFormatter>(Lifestyle.Singleton);
        c.RegisterConditional(
            typeof(IStaticFormatter<>),
            typeof(TypeNameFormatter<>),
            Lifestyle.Singleton,
            c => !c.Handled && c.ServiceType.GenericTypeArguments[0].IsAssignableTo(typeof(Exception)));
        c.RegisterConditional(
            typeof(IStaticFormatter<>),
            typeof(ToStringFormatter<>),
            Lifestyle.Singleton,
            c => !c.Handled);
    }
}
