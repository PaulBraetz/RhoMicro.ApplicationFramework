// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.Threading;

internal partial class PhotinoSynchronizationContext
{
    private class WorkItem
    {
        public required PhotinoSynchronizationContext SynchronizationContext { get; set; }
        public ExecutionContext? ExecutionContext { get; set; }
        public required SendOrPostCallback Callback { get; set; }
        public Object? State { get; set; }
    }
}
