using AutoFixture.Extensions.Http.Helpers;
using AutoFixture.Kernel;
using RichardSzalay.MockHttp;

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
}
