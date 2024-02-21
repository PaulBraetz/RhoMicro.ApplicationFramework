namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

internal sealed class PhotinoHttpHandler : DelegatingHandler
{
    private readonly PhotinoBlazorApp _app;

    //use this constructor if a handler is registered in DI to inject dependencies
    public PhotinoHttpHandler(PhotinoBlazorApp app) : this(app, null)
    {
    }

    //Use this constructor if a handler is created manually.
    //Otherwise, use DelegatingHandler.InnerHandler public property to set the next handler.
    public PhotinoHttpHandler(PhotinoBlazorApp app, HttpMessageHandler? innerHandler)
    {
        _app = app;

        //the last (inner) handler in the pipeline should be a "real" handler.
        //To make a HTTP request, create a HttpClientHandler instance.
        InnerHandler = innerHandler ?? new HttpClientHandler();
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var content = _app.WindowManager.HandleWebRequest(null, null, request.RequestUri?.AbsoluteUri, out var contentType);
        if(content != null)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(content)
            };

            response.Content.Headers.ContentType = contentType != null ?
                new MediaTypeHeaderValue(contentType)
                : null;

            return Task.FromResult(response);
        }

        return SendAsync(request, cancellationToken);
    }
}
