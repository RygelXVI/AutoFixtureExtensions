using Microsoft.Extensions.Options;

namespace Jouska.AutoFixture.Extensions.Options.Helpers;

public class TestOptionsSnapshot<TOptions> : IOptionsSnapshot<TOptions> where TOptions : class, new()
{
    public TOptions Value { get; }

    public TestOptionsSnapshot(TOptions value)
    {
        Value = value;
    }

    public TOptions Get(string? name) =>
        Value;
}