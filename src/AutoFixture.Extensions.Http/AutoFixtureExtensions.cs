using AutoFixture.Extensions.Http.Helpers;
using AutoFixture.Kernel;
using RichardSzalay.MockHttp;

namespace AutoFixture.Extensions.Http;

public static class AutoFixtureExtensions
{
    public static IFixture WithHttpClientFactory(this IFixture fixture, MockHttpMessageHandler mockHttpMessageHandler, string httpClientName)
    {
        var httpClient = mockHttpMessageHandler.ToHttpClient();
        var testHttpClientFactory = new TestHttpClientFactory(new KeyValuePair<string, HttpClient>(httpClientName, httpClient));
        fixture.Register(() => testHttpClientFactory);
        fixture.Customizations.Add(new TypeRelay(typeof(IHttpClientFactory), typeof(TestHttpClientFactory)));

        return fixture;
    }

    public static IFixture WithHttpClient(this IFixture fixture, MockHttpMessageHandler mockHttpMessageHandler)
    {
        var httpClient = mockHttpMessageHandler.ToHttpClient();
        fixture.Register(() => httpClient);
        return fixture;
    }
}
