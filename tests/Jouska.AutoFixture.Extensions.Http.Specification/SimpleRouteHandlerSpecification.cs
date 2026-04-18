using AutoFixture;
using Jouska.AutoFixture.Extensions.Http.Helpers;
using System.Net;

namespace Jouska.AutoFixture.Extensions.Http.Specification;

public class SimpleRouteHandlerSpecification
{
    [Fact]
    public async Task Can_register_simple_route_handler()
    {
        var handler = new SimpleRouteHandler();
        handler.WithRoute("/test", HttpStatusCode.Processing);

        var fixture = new Fixture().WithHttpClient(handler);

        var client = fixture.Freeze<HttpClientTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(client),
            () => Assert.NotNull(client.HttpClient)            
        );

        var response = await client.HttpClient.GetAsync("test", CancellationToken.None);

        Assert.Equal(HttpStatusCode.Processing, response.StatusCode);
    }

    [Fact]
    public void Can_register_simple_route_handler_with_factory()
    {
        var clientName = nameof(SimpleRouteHandler);

        var handler = new SimpleRouteHandler();
        var fixture = new Fixture().WithHttpClientFactory(clientName, handler);

        var factory = fixture.Freeze<IHttpClientFactory>();

        Assert.NotNull(factory);

        var client = factory.CreateClient(clientName);

        Assert.NotNull(client);
    }

    [Fact]
    public void Can_register_multiple_simple_route_handler_with_factory()
    {
        var clientName1 = "client1";
        var clientName2 = "client2";

        var handler1 = new SimpleRouteHandler();
        var handler2 = new SimpleRouteHandler();
        var fixture = new Fixture().WithHttpClientFactory((clientName1, handler1), (clientName2, handler2));

        var factory = fixture.Freeze<IHttpClientFactory>();

        Assert.NotNull(factory);

        var client1 = factory.CreateClient(clientName1);

        Assert.NotNull(client1);

        var client2 = factory.CreateClient(clientName2);

        Assert.NotNull(client2);
    }

    [Fact]
    public async Task Can_register_route_and_response_for_simple_route_handler_directly()
    {
        var fixture = new Fixture().WithHttpClient(("/all", HttpStatusCode.UnavailableForLegalReasons));

        var client = fixture.Freeze<HttpClient>();

        var request = new HttpRequestMessage(HttpMethod.Get, "http://local/all");
        var response = await client.SendAsync(request, CancellationToken.None);

        Assert.Equal(HttpStatusCode.UnavailableForLegalReasons, response.StatusCode);
    }
}

public class HttpClientFactoryTestSubject
{
    public HttpClientFactoryTestSubject(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public IHttpClientFactory HttpClientFactory { get; }
}

public class HttpClientTestSubject
{
    public HttpClientTestSubject(HttpClient httpClient)
    {
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    public HttpClient HttpClient { get; }
}