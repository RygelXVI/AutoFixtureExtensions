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
    /// <param name="mockHttpMessageHandler">the httpMessageHandler for the factory to use</param>
    /// <param name="httpClientName">the name of the httpclient</param>
    /// <returns>IFixture</returns>
    public static IFixture WithHttpClientFactory(this IFixture fixture, MockHttpMessageHandler mockHttpMessageHandler, string httpClientName)
    {
        var httpClient = mockHttpMessageHandler.ToHttpClient();
        var testHttpClientFactory = new TestHttpClientFactory(new KeyValuePair<string, HttpClient>(httpClientName, httpClient));
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
