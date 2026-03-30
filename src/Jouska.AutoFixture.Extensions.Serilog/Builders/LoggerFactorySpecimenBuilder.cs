using AutoFixture;
using AutoFixture.Kernel;
using Serilog;
using MEL = Microsoft.Extensions.Logging;

namespace Jouska.AutoFixture.Extensions.Serilog.Builders;

public class LoggerFactorySpecimenBuilder : ISpecimenBuilder
{
    private MEL.ILoggerFactory? _factory;

    public object Create(object request, ISpecimenContext context)
    {
        if (_factory == null)
        {
            var logger = context.Create<ILogger>();
            _factory = MEL.LoggerFactory.Create(builder => builder.AddSerilog(logger));
        }
        return _factory;
    }
}
