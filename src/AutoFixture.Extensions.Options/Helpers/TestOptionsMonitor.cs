using Microsoft.Extensions.Options;
using NSubstitute;

namespace AutoFixture.Extensions.Options.Helpers;

public class TestOptionsMonitor<TOptions> : IOptionsMonitor<TOptions> where TOptions : class, new()
{
    private Action<TOptions, string>? _listener;

    public TestOptionsMonitor(TOptions currentValue)
    {
        CurrentValue = currentValue;
    }

    public TOptions CurrentValue { get; private set; }

    public TOptions Get(string? name) =>
        CurrentValue;

    public void Set(TOptions value)
    {
        CurrentValue = value;
        _listener?.Invoke(value, null!);
    }

    public IDisposable OnChange(Action<TOptions, string> listener)
    {
        _listener = listener;
        return Substitute.For<IDisposable>();
    }
}
