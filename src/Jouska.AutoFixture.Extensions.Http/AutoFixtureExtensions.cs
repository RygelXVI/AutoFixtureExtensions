using Jouska.AutoFixture.Extensions.Http.Helpers;
using AutoFixture.Kernel;
using System.Net;
using AutoFixture;

namespace Jouska.AutoFixture.Extensions.Http;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Configures the fixture to use an HTTP client with the specified name and delegating handler.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="httpClientName">The name to associate with the created HTTP client.</param>
    /// <param name="delegatingHandler">The delegating handler to use for processing HTTP request and response messages. Cannot be null.</param>
    /// <returns>An updated fixture instance configured with an IHttpClientFactory test implementation.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, string httpClientName, DelegatingHandler delegatingHandler)
    {
        var httpClient = new Dictionary<string, HttpClient>()
        {
            { httpClientName, CreateHttpClient(delegatingHandler) }
        };

        return WithHttpClientFactory(fixture, httpClient);
    }

    /// <summary>
    /// Allows registration of multiple named http clients, using the specified delegating handlers, which the system under test can access via an IHttpClientFactory
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="httpClients">An array of tuples, each containing the name of the HTTP client and the delegating handler to be used for
    /// that client. The client name must not be null or empty. The delegating handler is used to configure the HTTP
    /// client's request pipeline.</param>
    /// <returns>An updated fixture instance configured with an IHttpClientFactory test implementation.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, params (string httpClientName, DelegatingHandler delegatingHandler)[] httpClients)
    {
        var clients = httpClients.ToDictionary(x => x.httpClientName, x => CreateHttpClient(x.delegatingHandler));

        return WithHttpClientFactory(fixture, clients);
    }

    /// <summary>
    /// Allows registration of multiple named MockHttpMessageHandler which the system under test can access via an IHttpClientFactory
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="httpClients">dictionary of httpClients to be registered</param>
    /// <returns>An updated fixture instance configured with an IHttpClientFactory test implementation.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, Dictionary<string, HttpClient> httpClients)
    {
        var testHttpClientFactory = new TestHttpClientFactory(httpClients);
        fixture.Register(() => testHttpClientFactory);
        fixture.Customizations.Add(new TypeRelay(typeof(IHttpClientFactory), typeof(TestHttpClientFactory)));

        return fixture;
    }

    /// <summary>
    /// Allows registration of an HttpClient, configured with the supplied delegating handler, which the system under test access via an HttpClient.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="delegatingHandler"></param>
    /// <returns>An updated fixture instance configured with an HttpClient.</returns>
    public static IFixture WithHttpClient(this IFixture fixture, DelegatingHandler delegatingHandler)
    {
        var httpClient = CreateHttpClient(delegatingHandler);
        fixture.Register(() => httpClient);
        return fixture;
    }

    /// <summary>
    /// Allows registration of an HttpClient, with a delegating handler configured with the specified routes and responses, which the system under test access via an HttpClient.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="routesAndResponses"></param>
    /// <returns>An updated fixture instance configured with an HttpClient.</returns>
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
