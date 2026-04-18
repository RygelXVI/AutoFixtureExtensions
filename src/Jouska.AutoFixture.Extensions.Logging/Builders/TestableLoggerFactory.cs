using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Jouska.AutoFixture.Extensions.Logging.Builders;

public sealed class TestableLoggerFactory : ILoggerFactory
{
    private readonly Dictionary<string, ILogger> _loggers = [];
    private readonly Action<FakeLogCollectorOptions>? _configureOptions;

    public TestableLoggerFactory()
    {        
    }

    public TestableLoggerFactory(Action<FakeLogCollectorOptions> configureOptions)
    {
        _configureOptions = configureOptions;
    }

    public FakeLogger? GetFakeLogger(string categoryName)
    {
        _loggers.TryGetValue(categoryName, out var logger);
        return logger as FakeLogger ?? default;
    }

    public FakeLogger? GetFakeLogger<T>() where T : class
    {
        var categoryName = typeof(T).FullName;
        return GetFakeLogger(categoryName!);
    }

    public void AddProvider(ILoggerProvider provider)
    {        
    }

    public ILogger CreateLogger(string categoryName)
    {
        if (!_loggers.TryGetValue(categoryName, out var logger))
        {
            if (_configureOptions != null)
            {
                var options = new FakeLogCollectorOptions();
                _configureOptions(options);
                logger = new FakeLogger(category: categoryName, collector: FakeLogCollector.Create(options));
            }
            else
            {
                logger = new FakeLogger(category: categoryName);
            }
            
            _loggers[categoryName] = logger;
        }

        return logger;
    }

    public void Dispose()
    {
    }
}
