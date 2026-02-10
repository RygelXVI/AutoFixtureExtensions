using AutoFixture.Extensions.Http.Helpers;
using AutoFixture.Kernel;
using Mockly;

namespace AutoFixture.Extensions.Http.Mockly;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Allows registration of a MockHttpMessageHandler which the system under test can access via an IHttpClientFactory, e.g. <br/><br/>
    /// <code>
    /// var httpMessageHandlerMock = new MockHttpMessageHandler();
    /// fixture.WithHttpClientFactory("client_1", httpMessageHandlerMock);
    /// </code>
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="httpClientName">the name of the httpclient</param>
    /// <param name="mockHttpMessageHandler">the httpMessageHandler for the factory to use</param>
    /// <returns>An updated fixture instance configured with an IHttpClientFactory test implementation.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, string httpClientName, HttpMock httpMock)
    {
        var httpClient = new Dictionary<string, HttpClient>()
        {
            { httpClientName, httpMock.GetClient() }
        };

        return WithHttpClientFactory(fixture, httpClient);
    }

    /// <summary>
    /// Allows registration of multiple named MockHttpMessageHandlers which the system under test can access via an IHttpClientFactory
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="mockHttpMessageHandler">the httpMessageHandler for the factory to use</param>
    /// <param name="httpClientName">the name of the httpclient</param>
    /// <returns>An updated fixture instance configured with an IHttpClientFactory test implementation.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, params (string httpClientName, HttpMock httpMock)[] httpClients)
    {
        var clients = httpClients.ToDictionary(x => x.httpClientName, x => x.httpMock.GetClient());

        return WithHttpClientFactory(fixture, clients);
    }

    /// <summary>
    /// Allows registration of multiple named MockHttpMessageHandler which the system under test can access via an IHttpClientFactory
    /// Uses a simple factory implementation for testing.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="httpClients">dictionary of MockHttpMessageHandlers to be registered as httpClients</param>
    /// <returns>An updated fixture instance configured with an IHttpClientFactory test implementation.</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, Dictionary<string, HttpMock> httpMessageHandlers)
    {
        var httpClients = httpMessageHandlers.ToDictionary(x => x.Key, x => x.Value.GetClient());

        return WithHttpClientFactory(fixture, httpClients);
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
    /// Allows registration of a MockHttpMessageHandler which the system under test access via an HttpClient.
    /// </summary>
    /// <param name="fixture">IFixture instance being configured</param>
    /// <param name="httpMock">the httpMessageHandler for the client to use</param>
    /// <returns>An updated fixture instance configured with an HttpClient.</returns>
    public static IFixture WithHttpClient(this IFixture fixture, HttpMock httpMock)
    {
        var httpClient = httpMock.GetClient();
        fixture.Register(() => httpClient);
        return fixture;
    }
}
