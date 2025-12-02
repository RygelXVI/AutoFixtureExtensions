using System.Net;
using System.Net.Http.Json;

namespace AutoFixture.Extensions.Http.Helpers;


// based on code sample from https://www.mostlylucid.net/blog/httpclient-testing-without-mocks
public class SimpleRouteHandler : DelegatingHandler
{
    private readonly Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>> _routes;

    public SimpleRouteHandler()
    {
        _routes = [];
    }

    public SimpleRouteHandler WithRoute(string path, HttpStatusCode status)
    {
        _routes[path] = _ => Task.FromResult(new HttpResponseMessage(status));
        return this;
    }

    public SimpleRouteHandler WithRoute(string path, object responseBody)
    {
        _routes[path] = _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseBody)
        });
        return this;
    }

    public SimpleRouteHandler WithRoute(string path, Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
    {
        _routes[path] = handler;
        return this;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var path = request.RequestUri?.AbsolutePath ?? "";

        if (_routes.TryGetValue(path, out var handler))
            return await handler(request);

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }
}
