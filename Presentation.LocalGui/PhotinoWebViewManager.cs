// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

using PhotinoNET;

/// <summary>
/// Integrates the <see cref="WebViewManager"/> into photino.
/// </summary>
public sealed class PhotinoWebViewManager : WebViewManager
{
    private readonly PhotinoWindow _window;
    private readonly Channel<String> _channel;

    // On Windows, we can't use a custom scheme to host the initial HTML,
    // because webview2 won't let you do top-level navigation to such a URL.
    // On Linux/Mac, we must use a custom scheme, because their webviews
    // don't have a way to intercept http:// scheme requests.
    internal static readonly String BlazorAppScheme = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? "http"
        : "app";

    internal static readonly String AppBaseUri = $"{BlazorAppScheme}://localhost/";

    internal PhotinoWebViewManager(PhotinoWindow window, IServiceProvider provider, Dispatcher dispatcher,
        IFileProvider fileProvider, JSComponentConfigurationStore jsComponents, IOptions<PhotinoBlazorAppConfiguration> config)
        : base(provider, dispatcher, config.Value.AppBaseUri, fileProvider, jsComponents, config.Value.HostPage)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));

        // Create a scheduler that uses one thread.
        var sts = new SynchronousTaskScheduler();

        _window.WebMessageReceived += (sender, message) =>
        {
            // On some platforms, we need to move off the browser UI thread
            _ = Task.Factory.StartNew(message =>
            {
                // TODO: Fix this. Photino should ideally tell us the URL that the message comes from so we
                // know whether to trust it. Currently it's hardcoded to trust messages from any source, including
                // if the webview is somehow navigated to an external URL.
                var messageOriginUrl = new Uri(AppBaseUri);

                MessageReceived(messageOriginUrl, (String)message!);
            }, message, CancellationToken.None, TaskCreationOptions.DenyChildAttach, sts);
        };

        //Create channel and start reader
        _channel = Channel.CreateUnbounded<String>(new UnboundedChannelOptions() { SingleReader = true, SingleWriter = false, AllowSynchronousContinuations = false });
        _ = Task.Run(MessagePump);
    }

    internal Stream? HandleWebRequest(Object? _0, String? _1, String? url, out String? contentType)
    {
        // It would be better if we were told whether or not this is a navigation request, but
        // since we're not, guess.
        var notNullUrl = url ?? String.Empty;
        var localPath = new Uri(notNullUrl).LocalPath;
        var hasFileExtension = localPath.LastIndexOf('.') > localPath.LastIndexOf('/');

        if(notNullUrl.StartsWith(AppBaseUri, StringComparison.Ordinal)
            && TryGetResponseContent(notNullUrl, !hasFileExtension, out _, out _,
                out var content, out var headers))
        {
            _ = headers.TryGetValue("Content-Type", out contentType);
            return content;
        } else
        {
            contentType = default;
            return null;
        }
    }
    /// <inheritdoc/>
    protected override void NavigateCore(Uri absoluteUri) =>
        _window.Load(absoluteUri);
    /// <inheritdoc/>
    protected override void SendMessage(String message)
    {
        while(!_channel.Writer.TryWrite(message))
        {
            Thread.Sleep(200);
        }
    }
    private async Task MessagePump()
    {
        var reader = _channel.Reader;
        try
        {
            while(true)
            {
                var message = await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: true);
                await _window.SendWebMessageAsync(message).ConfigureAwait(continueOnCapturedContext: true);
            }
        } catch(ChannelClosedException) { }
    }
    /// <inheritdoc/>
    protected override ValueTask DisposeAsyncCore()
    {
        //complete channel
        try
        {
            _channel.Writer.Complete();
#pragma warning disable CA1031 // Do not catch general exception types
        } catch { }
#pragma warning restore CA1031 // Do not catch general exception types

        //continue disposing
        return base.DisposeAsyncCore();
    }
}