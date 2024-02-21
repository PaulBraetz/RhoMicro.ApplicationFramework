// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.Threading.Tasks;

internal partial class PhotinoSynchronizationContext
{
    private class State
    {
        public Boolean IsBusy { get; set; } // Just for debugging
        public Object Lock { get; set; } = new Object();
        public Task Task { get; set; } = Task.CompletedTask;

        public override String ToString() =>
            $"{{ Busy: {IsBusy}, Pending Task: {Task} }}";
    }
}
