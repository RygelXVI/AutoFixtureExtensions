using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Jouska.AutoFixture.Extensions.Logging.Builders;

public class LoggerFactorySpecimenBuilder : ISpecimenBuilder
{
    private ILoggerFactory? _factory;
    private readonly Action<FakeLogCollectorOptions>? _configureOptions;

    public LoggerFactorySpecimenBuilder()
    {        
    }

    public LoggerFactorySpecimenBuilder(Action<FakeLogCollectorOptions> configureOptions)
    {
        _configureOptions = configureOptions;
    }

    public object Create(object request, ISpecimenContext context)
    {
        _factory ??= _configureOptions is null 
                ? new TestableLoggerFactory() 
                : new TestableLoggerFactory(_configureOptions);
        
        return _factory;
    }
}