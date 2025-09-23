using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;

namespace AutoFixture.Extensions.Logging.Builders;

public class LoggerFactorySpecimenBuilder : ISpecimenBuilder
{
    private ILoggerFactory? _factory;

    public object Create(object request, ISpecimenContext context)
    {
        if (_factory == null)
        {
            var logger = context.Create<ILogger>();
            _factory = LoggerFactory.Create(builder => builder.AddFakeLogging());
        }
        return _factory;
    }
}