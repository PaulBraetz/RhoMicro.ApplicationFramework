// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

internal class PhotinoDispatcher : Dispatcher
{
    private readonly PhotinoSynchronizationContext _context;

    public PhotinoDispatcher(PhotinoSynchronizationContext context)
    {
        _context = context;
        _context.UnhandledException += (sender, e) => OnUnhandledException(e);
    }

    public override Boolean CheckAccess() =>
        SynchronizationContext.Current == _context;

    public override Task InvokeAsync(Action workItem)
    {
        if(CheckAccess())
        {
            workItem();
            return Task.CompletedTask;
        }

        return _context.InvokeAsync(workItem);
    }

    public override Task InvokeAsync(Func<Task> workItem) =>
        CheckAccess() ? workItem() : _context.InvokeAsync(workItem);

    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem) =>
        CheckAccess() ? Task.FromResult(workItem()) : _context.InvokeAsync<TResult>(workItem);

    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem) =>
        CheckAccess() ? workItem() : _context.InvokeAsync<TResult>(workItem);
}
