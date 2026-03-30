using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Jouska.AutoFixture.Extensions.Logging.Builders;

public class LoggerFactorySpecimenBuilder : ISpecimenBuilder
{
    private readonly Action<FakeLogCollectorOptions>? _configureOptions;
    private ILoggerFactory? _factory;

    public LoggerFactorySpecimenBuilder()
    {        
    }

    public LoggerFactorySpecimenBuilder(Action<FakeLogCollectorOptions> configureOptions)
    {
        _configureOptions = configureOptions;
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (_configureOptions != null)
        {
            _factory ??= LoggerFactory.Create(builder => builder.AddFakeLogging(_configureOptions));
        }
        else
        {
            _factory ??= LoggerFactory.Create(builder => builder.AddFakeLogging());
        }

        return _factory;
    }
}