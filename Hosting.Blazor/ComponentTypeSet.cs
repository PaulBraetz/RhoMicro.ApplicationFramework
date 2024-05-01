namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Collections;
using System.Reflection;

using Microsoft.AspNetCore.Components;

using RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Represents a set of component types to be registered to a container.
/// </summary>
public sealed class ComponentTypeSet : ISet<Type>
{
    /// <summary>
    /// Initializes a new empty component set.
    /// </summary>
    public ComponentTypeSet() : this([]) { }
    private ComponentTypeSet(HashSet<Type> componentTypes) => _componentTypes = componentTypes;
    private readonly HashSet<Type> _componentTypes;

    /// <summary>
    /// Adds a component type to the set.
    /// </summary>
    /// <param name="item">The component type to add.</param>
    public ComponentTypeSet Add(Type item)
    {
        ( (ICollection<Type>)this ).Add(item);

        return this;
    }
    /// <summary>
    /// Adds all component types from an assembly to the set.
    /// </summary>
    /// <param name="componentsAssembly">The assembly containing component types to add.</param>
    public ComponentTypeSet Add(Assembly componentsAssembly)
    {
        ArgumentNullException.ThrowIfNull(componentsAssembly);

        componentsAssembly
            .GetTypes()
            .Where(IsValidComponentType)
            .ForEach(( (ICollection<Type>)this ).Add);

        return this;
    }
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
        new(componentTypes.Where(IsValidComponentType).ToHashSet());

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

    private static Boolean IsValidComponentType(Type t)
    {
        ArgumentNullException.ThrowIfNull(t);

        var result = !t.IsAbstract
            && t.IsClass
            && t.IsAssignableTo(typeof(IComponent))
            && t.GetCustomAttribute<ExcludeComponentFromContainerAttribute>(inherit: false) == null;

        return result;
    }
    #region ISet<Type> implementation
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Boolean ISet<Type>.Add(Type item) => ( (ISet<Type>)_componentTypes ).Add(item);
    void ISet<Type>.ExceptWith(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).ExceptWith(other);
    void ISet<Type>.IntersectWith(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).IntersectWith(other);
    Boolean ISet<Type>.IsProperSubsetOf(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).IsProperSubsetOf(other);
    Boolean ISet<Type>.IsProperSupersetOf(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).IsProperSupersetOf(other);
    Boolean ISet<Type>.IsSubsetOf(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).IsSubsetOf(other);
    Boolean ISet<Type>.IsSupersetOf(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).IsSupersetOf(other);
    Boolean ISet<Type>.Overlaps(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).Overlaps(other);
    Boolean ISet<Type>.SetEquals(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).SetEquals(other);
    void ISet<Type>.SymmetricExceptWith(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).SymmetricExceptWith(other);
    void ISet<Type>.UnionWith(IEnumerable<Type> other) => ( (ISet<Type>)_componentTypes ).UnionWith(other);
    public void Clear() => ( (ICollection<Type>)_componentTypes ).Clear();
    public Boolean Contains(Type item) => ( (ICollection<Type>)_componentTypes ).Contains(item);
    void ICollection<Type>.CopyTo(Type[] array, Int32 arrayIndex) => ( (ICollection<Type>)_componentTypes ).CopyTo(array, arrayIndex);
    public Boolean Remove(Type item) => ( (ICollection<Type>)_componentTypes ).Remove(item);

    public Int32 Count => ( (ICollection<Type>)_componentTypes ).Count;

    Boolean ICollection<Type>.IsReadOnly => ( (ICollection<Type>)_componentTypes ).IsReadOnly;

    public IEnumerator<Type> GetEnumerator() => ( (IEnumerable<Type>)_componentTypes ).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ( (IEnumerable)_componentTypes ).GetEnumerator();
    void ICollection<Type>.Add(Type item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if(!IsValidComponentType(item))
        {
            throw new ArgumentOutOfRangeException(
                nameof(item),
                item,
                $"{nameof(item)} is not a valid compoennt type.");
        }

        _ = _componentTypes.Add(item);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion
}
