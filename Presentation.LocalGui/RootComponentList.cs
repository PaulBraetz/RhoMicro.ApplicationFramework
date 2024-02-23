namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents the collection of root components for a photino blazor app.
/// </summary>
public sealed class RootComponentList
{
    private readonly List<(Type componentType, String domElementSelector)> _components = [];
    internal IEnumerable<(Type, String)> Enumerate() => _components;

    /// <summary>
    /// Adds a component to the list of root components.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    /// <param name="selector">The css selector by which to replace the root components html stand-in element.</param>
    public void Add<TComponent>(String selector)
        where TComponent : IComponent
    {
        ArgumentNullException.ThrowIfNull(selector);

        _components.Add((typeof(TComponent), selector));
    }

    /// <summary>
    /// Adds a component to the list of root components.
    /// </summary>
    /// <param name="componentType">The type of the component to add.</typeparam>
    /// <param name="selector">The css selector by which to replace the root components html stand-in element.</param>
    public void Add(Type componentType, String selector)
    {
        ArgumentNullException.ThrowIfNull(componentType);
        ArgumentNullException.ThrowIfNull(selector);

        if(!componentType.IsAssignableTo(typeof(IComponent)))
            throw new ArgumentException($"{nameof(componentType)} of {componentType} does not implement {typeof(IComponent)}", nameof(componentType));

        _components.Add((componentType, selector));
    }
}
