// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;
using System.Threading.Tasks;

internal partial class PhotinoSynchronizationContext
{
    private class PhotinoSynchronizationTaskCompletionSource<TCallback, TResult>(TCallback callback) : TaskCompletionSource<TResult>
    {
        public TCallback Callback { get; } = callback;
    }
}
