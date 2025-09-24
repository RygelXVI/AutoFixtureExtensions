using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;

namespace AutoFixture.Extensions.Logging.Builders;

public class LoggerFactorySpecimenBuilder : ISpecimenBuilder
{
    private ILoggerFactory? _factory;

    public object Create(object request, ISpecimenContext context)
    {
        _factory ??= LoggerFactory.Create(builder => builder.AddFakeLogging());
        return _factory;
    }
}