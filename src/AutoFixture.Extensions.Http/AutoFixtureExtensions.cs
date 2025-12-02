using AutoFixture.Extensions.Http.Helpers;
using AutoFixture.Kernel;
using RichardSzalay.MockHttp;
using System.Net;

namespace AutoFixture.Extensions.Http;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Allows registration of a MockHttpMessageHandler which the system under test can access via an IHttpClientFactory, e.g. <br/><br/>
    /// <code>
    /// var httpMessageHandlerMock = new MockHttpMessageHandler();
    /// fixture.WithHttpClientFactory(httpMessageHandlerMock, "client_1");
    /// </code>
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture"></param>
    /// <param name="httpClientName">the name of the httpclient</param>
    /// <param name="mockHttpMessageHandler">the httpMessageHandler for the factory to use</param>
    /// <returns>IFixture</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, string httpClientName, MockHttpMessageHandler mockHttpMessageHandler)
    {
        var httpClient = new Dictionary<string, HttpClient>()
        {
            { httpClientName, mockHttpMessageHandler.ToHttpClient() }
        };

        return WithHttpClientFactory(fixture, httpClient);
    }

    /// <summary>
    /// Configures the fixture to use an HTTP client with the specified name and delegating handler.
    /// </summary>
    /// <param name="httpClientName">The name to associate with the created HTTP client.</param>
    /// <param name="delegatingHandler">The delegating handler to use for processing HTTP request and response messages. Cannot be null.</param>
    /// <returns>An updated fixture instance configured with the specified HTTP client and handler.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, string httpClientName, DelegatingHandler delegatingHandler)
    {
        var httpClient = new Dictionary<string, HttpClient>()
        {
            { httpClientName, CreateHttpClient(delegatingHandler) }
        };

        return WithHttpClientFactory(fixture, httpClient);
    }

    /// <summary>
    /// Allows registration of multiple named MockHttpMessageHandlers which the system under test can access via an IHttpClientFactory
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture"></param>
    /// <param name="mockHttpMessageHandler">the httpMessageHandler for the factory to use</param>
    /// <param name="httpClientName">the name of the httpclient</param>
    /// <returns>IFixture</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, params (string httpClientName, MockHttpMessageHandler mockHttpMessageHandler)[] httpClients)
    {
        var clients = httpClients.ToDictionary(x => x.httpClientName, x => x.mockHttpMessageHandler.ToHttpClient());

        return WithHttpClientFactory(fixture, clients);
    }

    /// <summary>
    /// Configures the fixture to use custom HTTP clients created from the specified names and delegating handlers.
    /// </summary>
    /// <remarks>Each tuple in the array results in a new HttpClient instance associated with the
    /// given name and handler. This method is useful for testing scenarios that require custom HTTP client
    /// configurations.</remarks>
    /// <param name="httpClients">An array of tuples, each containing the name of the HTTP client and the delegating handler to be used for
    /// that client. The client name must not be null or empty. The delegating handler is used to configure the HTTP
    /// client's request pipeline.</param>
    /// <returns>An updated fixture instance configured with the specified HTTP clients.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, params (string httpClientName, DelegatingHandler delegatingHandler)[] httpClients)
    {
        var clients = httpClients.ToDictionary(x => x.httpClientName, x => CreateHttpClient(x.delegatingHandler));

        return WithHttpClientFactory(fixture, clients);
    }

    /// <summary>
    /// Allows registration of multiple named MockHttpMessageHandler which the system under test can access via an IHttpClientFactory
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture"></param>
    /// <param name="httpClients">dictionary of MockHttpMessageHandlers to be registered as httpClients</param>
    /// <returns></returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, Dictionary<string, MockHttpMessageHandler> httpMessageHandlers)
    {
        var httpClients = httpMessageHandlers.ToDictionary(x => x.Key, x => x.Value.ToHttpClient());

        return WithHttpClientFactory(fixture, httpClients);
    }

    /// <summary>
    /// Allows registration of multiple named MockHttpMessageHandler which the system under test can access via an IHttpClientFactory
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture"></param>
    /// <param name="httpClients">dictionary of httpClients to be registered</param>
    /// <returns></returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, Dictionary<string, HttpClient> httpClients)
    {
        var testHttpClientFactory = new TestHttpClientFactory(httpClients);
        fixture.Register(() => testHttpClientFactory);
        fixture.Customizations.Add(new TypeRelay(typeof(IHttpClientFactory), typeof(TestHttpClientFactory)));

        return fixture;
    }

    /// <summary>
    /// Allows registration of a MockHttpMessageHandler which the system under test access via an HttpClient.
    /// </summary>
    /// <param name="mockHttpMessageHandler">the httpMessageHandler for the client to use</param>
    /// <returns>IFixture</returns>
    public static IFixture WithHttpClient(this IFixture fixture, MockHttpMessageHandler mockHttpMessageHandler)
    {
        var httpClient = mockHttpMessageHandler.ToHttpClient();
        fixture.Register(() => httpClient);
        return fixture;
    }

    public static IFixture WithHttpClient(this IFixture fixture, DelegatingHandler delegatingHandler)
    {
        var httpClient = CreateHttpClient(delegatingHandler);
        fixture.Register(() => httpClient);
        return fixture;
    }

    public static IFixture WithHttpClient(this IFixture fixture, params (string route, HttpStatusCode responseStatus)[] routesAndResponses)
    {
        var testHandler = new SimpleRouteHandler();

        foreach (var (route, responseStatus) in routesAndResponses)
        {
            testHandler.WithRoute(route, responseStatus);
        }

        var httpClient = CreateHttpClient(testHandler);
        fixture.Register(() => httpClient);
        return fixture;
    }

    private static HttpClient CreateHttpClient(DelegatingHandler delegatingHandler)
    {
        var result = new HttpClient(delegatingHandler)
        {
            BaseAddress = new Uri("http://local/")
        };
        return result;
    }
}
