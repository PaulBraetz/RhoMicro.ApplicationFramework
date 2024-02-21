﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

/// <summary>
/// Configures root components for a photino blazor window.
/// </summary>
internal sealed class BlazorWindowRootComponents : IJSComponentConfiguration
{
    private readonly PhotinoWebViewManager _manager;

    internal BlazorWindowRootComponents(PhotinoWebViewManager manager, JSComponentConfigurationStore jsComponents)
    {
        _manager = manager;
        JSComponents = jsComponents;
    }

    public JSComponentConfigurationStore JSComponents { get; }

    /// <summary>
    /// Adds a root component to the window.
    /// </summary>
    /// <param name="typeComponent">The type of component to add.</param>
    /// <param name="selector">A CSS selector describing where the component should be added in the host page.</param>
    /// <param name="parameters">An optional dictionary of parameters to pass to the component.</param>
    public void Add(Type typeComponent, String selector, IDictionary<String, Object>? parameters = null)
    {
        var parameterView = parameters == null
            ? ParameterView.Empty
            : ParameterView.FromDictionary(parameters!);

        // Dispatch because this is going to be async, and we want to catch any errors
        _ = _manager.Dispatcher.InvokeAsync(async () => await _manager.AddRootComponentAsync(typeComponent, selector, parameterView).ConfigureAwait(continueOnCapturedContext: true));
    }
}
