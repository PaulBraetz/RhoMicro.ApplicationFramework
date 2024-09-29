namespace RhoMicro.ApplicationFramework.Composition;

using System;
using System.Collections.Generic;

using RhoMicro.ApplicationFramework.Common.Abstractions;

using SimpleInjector;

/// <summary>
/// Context object used when adding sequential interceptors to a container
/// collection.
/// </summary>
public readonly struct InterceptorAppendContext : IEquatable<InterceptorAppendContext>
{
    internal InterceptorAppendContext(Container container, Lifestyle? lifestyle)
    {
        _container = container;
        _lifestyle = lifestyle;
    }

    private readonly Container _container;
    private readonly Lifestyle? _lifestyle;
    /// <summary>
    /// Appends an interceptor to the containers collection via <see
    /// cref="ContainerCollectionRegistrator.Append{TService,
    /// TImplementation}()"/>. If a default lifestyle was provided, the
    /// interceptor will be registered via <see
    /// cref="ContainerCollectionRegistrator.Append{TService,
    /// TImplementation}(Lifestyle)"/> instead, using the default lifestyle
    /// provided.
    /// </summary>
    /// <typeparam name="TInterceptor">
    /// The type of interceptor implementation to add.
    /// </typeparam>
    /// <typeparam name="T">
    /// The type of parameter intercepted by instances of <see
    /// cref="Append{TInterceptor, T}()"/>.
    /// </typeparam>
    /// <returns>
    /// A reference to the context, for chaining of further method calls.
    /// </returns>
    public InterceptorAppendContext Append<TInterceptor, T>()
        where TInterceptor : class, IInterceptor<T>
    {
        if(_lifestyle is { })
            _container.Collection.Append<IInterceptor<T>, TInterceptor>(_lifestyle);
        else
            _container.Collection.Append<IInterceptor<T>, TInterceptor>();

        return this;
    }
    /// <summary>
    /// Appends an interceptor to the containers collection via <see
    /// cref="ContainerCollectionRegistrator.Append{TService,
    /// TImplementation}(Lifestyle)"/>.
    /// </summary>
    /// <param name="lifestyle">
    /// The lifestyle with which to register the interceptor.
    /// </param>
    /// <typeparam name="TInterceptor">
    /// The type of interceptor implementation to add.
    /// </typeparam>
    /// <typeparam name="T">
    /// The type of parameter intercepted by instances of <see
    /// cref="Append{TInterceptor, T}()"/>.
    /// </typeparam>
    /// <returns>
    /// A reference to the context, for chaining of further method calls.
    /// </returns>
    public InterceptorAppendContext Append<TInterceptor, T>(Lifestyle lifestyle)
        where TInterceptor : class, IInterceptor<T>
    {
        _container.Collection.Append<IInterceptor<T>, TInterceptor>(lifestyle);
        return this;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public override Boolean Equals(Object? obj) => obj is InterceptorAppendContext context && Equals(context);
    public Boolean Equals(InterceptorAppendContext other) => EqualityComparer<Container>.Default.Equals(_container, other._container) && EqualityComparer<Lifestyle?>.Default.Equals(_lifestyle, other._lifestyle);
    public override Int32 GetHashCode() => HashCode.Combine(_container, _lifestyle);
    public static Boolean operator ==(InterceptorAppendContext left, InterceptorAppendContext right) => left.Equals(right);
    public static Boolean operator !=(InterceptorAppendContext left, InterceptorAppendContext right) => !( left  ==  right );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
