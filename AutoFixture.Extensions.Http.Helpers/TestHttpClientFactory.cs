

namespace AutoFixture.Extensions.Http.Helpers;

public class TestHttpClientFactory : IHttpClientFactory
{

    private readonly Dictionary<string, HttpClient> _clients;

    public TestHttpClientFactory(Dictionary<string, HttpClient> clients)
    {
        _clients = clients;
    }

    public TestHttpClientFactory(params KeyValuePair<string, HttpClient>[] clients)
    {
        _clients = clients.ToDictionary();
    }

    public HttpClient CreateClient(string name)
    {
        if (_clients.TryGetValue(name, out var client))
        {
            return client;
        }

        // Microsoft's default factory will create a new client if the cache does not have an entry for
        // the name... we probably don't need to do this for most unit testing scenarios
        return default!;
    }
}
