using AutoFixture.Kernel;
using Serilog;
using Serilog.Sinks.InMemory;

namespace AutoFixture.Extensions.Serilog.Builders;

public class SerilogLoggerSpecimenBuilder : ISpecimenBuilder
{
    private readonly Lazy<ILogger> _logger;

    public SerilogLoggerSpecimenBuilder()
    {
        _logger = new Lazy<ILogger>(() => new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.InMemory()
            .WriteTo.Debug()
            .CreateLogger());
    }

    public object Create(object request, ISpecimenContext context)
    {
        return _logger.Value;
    }
}
