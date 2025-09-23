using Microsoft.Extensions.Options;

namespace AutoFixture.Extensions.Options.Helpers;

public class TestOptionsFactory<TOptions> : IOptionsFactory<TOptions> where TOptions : class
{
    public TestOptionsFactory()
    {
        
    }

    public TOptions Create(string name)
    {
        throw new NotImplementedException();
    }
}
