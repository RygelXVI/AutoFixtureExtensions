using AutoFixture;
using Mockly;

namespace Jouska.AutoFixture.Extensions.Http.Mockly.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public async Task Can_register_httpmock_with_mockly_http_client_factory()
    {
        var path = "/api/users/*";
        var expected = "blah blah blah";
        var httpMock = new HttpMock();
        httpMock
            .ForGet()
            .WithPath(path)
            .RespondsWithContent(expected);

        var fixture = new Fixture().WithHttpClientFactory(httpMock);

        var testSubject = fixture.Freeze<HttpClientFactoryTestSubject>();

        Assert.NotNull(testSubject);

        var actual = await testSubject.HttpClient.GetAsync(path, TestContext.Current.CancellationToken);
        var content = await actual.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal(expected, content);
    }

    [Fact]
    public async Task Can_register_single_http_mock_as_named_client()
    {
        var httpMock = new HttpMock();
        httpMock
            .ForGet()
            .RespondsWithStatus(System.Net.HttpStatusCode.UseProxy);

        var clientName1 = "client1";

        var fixture = new Fixture().WithHttpClientFactory(clientName1, httpMock);

        var factory = fixture.Freeze<IHttpClientFactory>();

        var httpClient = factory.CreateClient(clientName1);

        Assert.NotNull(httpClient);

        var actual = await httpClient.GetAsync("", TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.UseProxy, actual.StatusCode);

        var otherClient = factory.CreateClient("other");

        Assert.Null(otherClient);
    }

    [Fact]
    public async Task Can_register_multiple_httpmocks_as_named_clients()
    {
        var clientName1 = "client1";
        var httpMock = new HttpMock();
        httpMock
            .ForGet()
            .RespondsWithStatus(System.Net.HttpStatusCode.UseProxy);

        var clientName2 = "client2";
        var httpMock2 = new HttpMock();
        httpMock2
            .ForGet()
            .RespondsWithStatus(System.Net.HttpStatusCode.PreconditionFailed);

        var fixture = new Fixture().WithHttpClientFactory((clientName1, httpMock), (clientName2, httpMock2));

        var factory = fixture.Freeze<IHttpClientFactory>();

        var client1 = factory.CreateClient(clientName1);
        var actual = await client1.GetAsync("", TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.UseProxy, actual.StatusCode);

        var client2 = factory.CreateClient(clientName2);
        var actual2 = await client2.GetAsync("!", TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, actual2.StatusCode);
    }

    [Fact]
    public async Task Can_register_httpmock_as_client()
    {
        var httpMock = new HttpMock();
        httpMock
            .ForGet()
            .RespondsWithStatus(System.Net.HttpStatusCode.PreconditionFailed);

        var fixture = new Fixture().WithHttpClient(httpMock);

        var testSubject2 = fixture.Freeze<HttpClientTestSubject>();

        Assert.NotNull(testSubject2);

        var actual = await testSubject2.HttpClient.GetAsync("", TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, actual.StatusCode);
    }

    [Fact]
    public async Task Can_register_dictionary_of_httpmocks()
    {
        var httpMock = new HttpMock();
        httpMock
            .ForGet()
            .RespondsWithStatus(System.Net.HttpStatusCode.PreconditionFailed);
        
        var httpMock2 = new HttpMock();
        httpMock2
            .ForGet()
            .RespondsWithStatus(System.Net.HttpStatusCode.UseProxy);

        var clients = new Dictionary<string, HttpMock>
        {
            { NamedHttpClientFactoryTestSubject.ClientName, httpMock },
            {  "other", httpMock2  }
        };

        var fixture = new Fixture().WithHttpClientFactory(clients);

        var testSubject = fixture.Freeze<NamedHttpClientFactoryTestSubject>();

        Assert.NotNull(testSubject);

        var actual = await testSubject.HttpClient.GetAsync("", TestContext.Current.CancellationToken);

        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, actual.StatusCode);
    }
}

public class HttpClientFactoryTestSubject
{
    public HttpClientFactoryTestSubject(IHttpClientFactory httpClientFactory)
    {
        HttpClient = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public HttpClient HttpClient { get; private set; }
}

public class HttpClientTestSubject
{
    public HttpClientTestSubject(HttpClient httpClient)
    {
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public HttpClient HttpClient { get; }
}

public class NamedHttpClientFactoryTestSubject
{
    public NamedHttpClientFactoryTestSubject(IHttpClientFactory httpClientFactory)
    {
        HttpClient = httpClientFactory?.CreateClient(ClientName) ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public static string ClientName => "Mine";

    public HttpClient HttpClient { get; private set; }
}
