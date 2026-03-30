using Microsoft.Extensions.Options;

namespace Jouska.AutoFixture.Extensions.Options.Helpers;

public class TestOptionsMonitor<TOptions> : IOptionsMonitor<TOptions>
{
    private readonly List<Action<TOptions, string>> _listeners;

    public TestOptionsMonitor(TOptions initialValue)
    {
        _listeners = [];
        CurrentValue = initialValue;
    }

    public TOptions CurrentValue { get; private set; }
    public TOptions Get(string? name) => CurrentValue;

    public IDisposable OnChange(Action<TOptions, string> listener)
    {
        _listeners.Add(listener);
        return new ActionDisposable(() => _listeners.Remove(listener));
    }

    public void UpdateOptions(TOptions options)
    {
        CurrentValue = options;
        _listeners.ForEach(listener => listener(options, string.Empty));
    }

    public sealed class ActionDisposable : IDisposable
    {
        readonly Action _action;

        public ActionDisposable(Action action) => _action = action;

        public void Dispose() => _action();
    }
}