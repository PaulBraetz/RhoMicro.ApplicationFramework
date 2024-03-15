namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using System;
using System.Collections;
using System.Reflection;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents a set of component types to be registered to a container.
/// </summary>
public sealed class ComponentTypeSet : IEnumerable<Type>
{
    private ComponentTypeSet(IEnumerable<Type> componentTypes) => _componentTypes = componentTypes;
    private readonly IEnumerable<Type> _componentTypes;

    /// <summary>
    /// Creates a new component type set.
    /// </summary>
    /// <param name="componentTypes">
    /// The collection of types from which to filter component types.
    /// </param>
    /// <returns>
    /// A new component type set.
    /// </returns>
    public static ComponentTypeSet Create(IEnumerable<Type> componentTypes) =>
        new(componentTypes.Where(t =>
               !t.IsAbstract
            && t.IsClass
            && ( t.IsConstructedGenericType || !t.IsGenericType )
            && t.IsAssignableTo(typeof(IComponent))
            && t.GetCustomAttribute<ExcludeComponentFromContainerAttribute>(inherit: false) == null)
        .ToHashSet());

    /// <summary>
    /// Creates a new component type set.
    /// </summary>
    /// <param name="componentAssemblies">
    /// The collection of assemblies from which to filter component types.
    /// </param>
    /// <returns>
    /// A new component type set.
    /// </returns>
    public static ComponentTypeSet Create(IEnumerable<Assembly> componentAssemblies) =>
        Create(componentAssemblies.SelectMany(a => a.GetTypes()));

    /// <summary>
    /// Creates a new component type set.
    /// </summary>
    /// <param name="componentTypes">
    /// The collection of types from which to filter component types.
    /// </param>
    /// <param name="componentAssemblies">
    /// The collection of assemblies from which to filter component types.
    /// </param>
    /// <returns>
    /// A new component type set.
    /// </returns>
    public static ComponentTypeSet Create(IEnumerable<Type> componentTypes, IEnumerable<Assembly> componentAssemblies) =>
        Create(componentTypes.Concat(componentAssemblies.SelectMany(a => a.GetTypes())));

    /// <inheritdoc/>
    public IEnumerator<Type> GetEnumerator() => _componentTypes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)_componentTypes ).GetEnumerator();
}
