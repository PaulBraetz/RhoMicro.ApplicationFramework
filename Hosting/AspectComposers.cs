namespace RhoMicro.ApplicationFramework.Composition;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RhoMicro.ApplicationFramework.Aspects.Abstractions;
using RhoMicro.ApplicationFramework.Aspects.Decorators;
using RhoMicro.ApplicationFramework.Aspects.Services;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Common.Formatters;

using SimpleInjector;

/// <summary>
/// Contains object graph definitions for cross-cutting concerns.
/// </summary>
public static class AspectComposers
{
    /// <summary>
    /// Gets a composer able to compose common aspects using the <see cref="Lifestyle.Scoped"/> lifestyle.
    /// </summary>
    public static IComposer Default { get; } = Create(Lifestyle.Scoped, CommonAspects.All);
    /// <summary>
    /// Gets a composer able to compose common aspects using the lifestyle provided.
    /// </summary>
    public static IComposer Create(Lifestyle lifestyle, CommonAspects aspects = CommonAspects.All) => Composer.Create(c => RegisterCoreAspects(c, lifestyle, aspects));
    /// <summary>
    /// Gets a composer adding the <see cref="SynchronizationContextDiscardingDecorator{TRequest, TResult}"/> to the container.
    /// </summary>
    public static IComposer SynchronizationContextDiscardingDecoratorComposer { get; } =
        Composer.Create(c => c.RegisterDecorator(typeof(IService<,>), typeof(SynchronizationContextDiscardingDecorator<,>), Lifestyle.Scoped));
    private static void RegisterCoreAspects(Container c, Lifestyle lifestyle, CommonAspects aspects)
    {
        RegisterFormatters(c);
        RegisterLoggingService(c);
        RegisterLoggingDecorators(c, lifestyle, aspects);
    }
    private static void RegisterLoggingDecorators(Container c, Lifestyle lifestyle, CommonAspects aspects)
    {
        if(aspects.HasFlag(CommonAspects.ExecutionTime))
            c.RegisterDecorator(typeof(IService<,>), typeof(ExecutionTimeLoggingServiceDecorator<,>), lifestyle);
        if(aspects.HasFlag(CommonAspects.Timestamp))
            c.RegisterDecorator(typeof(IService<,>), typeof(TimestampLoggingServiceDecorator<,>), lifestyle);
        if(aspects.HasFlag(CommonAspects.Exception))
            c.RegisterDecorator(typeof(IService<,>), typeof(ExceptionLoggingServiceDecorator<,>), lifestyle);
        if(aspects.HasFlag(CommonAspects.ThreadId))
            c.RegisterDecorator(typeof(IService<,>), typeof(ThreadIdLoggingServiceDecorator<,>), lifestyle);
        if(aspects.HasFlag(CommonAspects.Execution))
            c.RegisterDecorator(typeof(IService<,>), typeof(ExecutionLoggingServiceDecorator<,>), lifestyle);
        if(aspects.HasFlag(CommonAspects.ServiceType))
        {
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
        }, lifestyle, ctx => true);
        }
    }

    private static void RegisterLoggingService(Container c)
    {
        c.Register(() => new LoggingService(c.GetRequiredService<ILoggerFactory>().CreateLogger<LoggingService>()), Lifestyle.Singleton);
        c.Register<ILoggingService>(c.GetRequiredService<LoggingService>, Lifestyle.Singleton);
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
