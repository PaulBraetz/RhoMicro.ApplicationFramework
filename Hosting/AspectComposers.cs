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
/// Contains object graph definitions for cross-cutting concerns.
/// </summary>
public static class AspectComposers
{
    /// <summary>
    /// Gets a composer adding the <see cref="SynchronizationContextDiscardingDecorator{TRequest, TResult}"/> to the container.
    /// </summary>
    public static IComposer SynchronizationContextDiscardingDecoratorComposer { get; } =
        Composer.Create(c => c.RegisterDecorator(typeof(IService<,>), typeof(SynchronizationContextDiscardingDecorator<,>), Lifestyle.Scoped));
    /// <summary>
    /// Gets a composer able to compose common aspects using the <see cref="Lifestyle.Scoped"/> lifestyle.
    /// </summary>
    public static IComposer Default { get; } = CreateDefault(Lifestyle.Scoped, CommonAspects.All);
    /// <summary>
    /// Gets a composer for registering default formatters.
    /// </summary>
    public static IComposer Formatters { get; } = Composer.Create(c =>
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
    });
    /// <summary>
    /// Creates a composer that registers interceptors to container collections, to be resolved via <see
    /// cref="AggregateInterceptor{T}"/>. The interceptor aggregate will be
    /// registered using the containers default lifestyle.
    /// </summary>
    public static IComposer Interceptors { get; } =
        Composer.Create(c =>
        {
            c.Collection.Register(typeof(IInterceptor<>));
            c.RegisterConditional(typeof(IInterceptor<>), typeof(AggregateInterceptor<>), ctx => !ctx.Handled);
        });
    /// <summary>
    /// Gets a composer able to compose common aspects using the lifestyle provided.
    /// </summary>
    public static IComposer CreateDefault(Lifestyle lifestyle, CommonAspects aspects = CommonAspects.All, Action<InterceptorAppendContext>? appendInterceptors = null) => 
        Composer.Create(
        CreateForInterceptors(lifestyle, appendInterceptors ?? ( static c => { } )),
        Formatters,
        CreateForLoggingDecorators(lifestyle, aspects));
    /// <summary>
    /// Creates a composer that registers interceptors to container collections, to be resolved via <see
    /// cref="AggregateInterceptor{T}"/>. The interceptors will be
    /// registered using the containers default lifestyle.
    /// </summary>
    /// <param name="append">
    /// The callback to invoke when appending interceptors to container
    /// collections.
    /// </param>
    public static IComposer CreateForInterceptors(Action<InterceptorAppendContext> append)
    {
        ArgumentNullException.ThrowIfNull(append);
        return Interceptors +
            Composer.Create(container => append.Invoke(new(container, null)));
    }
    /// <summary>
    /// Creates a composer that registers interceptors to container collections, to be resolved via <see
    /// cref="AggregateInterceptor{T}"/>.
    /// </summary>
    /// <param name="lifestyle">
    /// The default lifestyle of interceptors added via <paramref
    /// name="append"/>.
    /// </param>
    /// <param name="append">
    /// The callback to invoke when appending interceptors to container
    /// collections.
    /// </param>
    public static IComposer CreateForInterceptors(Lifestyle lifestyle, Action<InterceptorAppendContext> append)
    {
        ArgumentNullException.ThrowIfNull(append);

        return CreateForInterceptors(lifestyle) +
            Composer.Create(container => append.Invoke(new(container, lifestyle)));
    }
    /// <summary>
    /// Creates a composer that registers interceptors to container collections, to be resolved via <see
    /// cref="AggregateInterceptor{T}"/>.
    /// </summary>
    /// <param name="lifestyle">
    /// The lifestyle to register <see cref="AggregateInterceptor{T}"/> with.
    /// </param>
    public static IComposer CreateForInterceptors(Lifestyle lifestyle) =>
        Composer.Create(c =>
        {
            c.Collection.Register(typeof(IInterceptor<>));
            c.RegisterConditional(typeof(IInterceptor<>), typeof(AggregateInterceptor<>), lifestyle, ctx => !ctx.Handled);
        });
    /// <summary>
    /// Creates a composer for registering logging decorators.
    /// </summary>
    /// <param name="lifestyle">The lifestyle to register decorators with.</param>
    /// <param name="aspects">The aspects to register.</param>
    /// <returns>A new composer capable of registering the decorators specified.</returns>
    public static IComposer CreateForLoggingDecorators(Lifestyle lifestyle, CommonAspects aspects) =>
        Composer.Create(c =>
        {
            c.Register(() => new LoggingService(c.GetRequiredService<ILoggerFactory>().CreateLogger<LoggingService>()), Lifestyle.Singleton);
            c.Register<ILoggingService>(c.GetRequiredService<LoggingService>, Lifestyle.Singleton);

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
        });
}
