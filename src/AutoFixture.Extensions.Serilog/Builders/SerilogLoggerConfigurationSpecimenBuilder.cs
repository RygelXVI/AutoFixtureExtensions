using AutoFixture.Kernel;
using Serilog;
using Serilog.Sinks.InMemory;

namespace AutoFixture.Extensions.Serilog.Builders;

public class SerilogLoggerConfigurationSpecimenBuilder : ISpecimenBuilder
{
    private readonly ExactTypeSpecification _loggerConfigurationSpecification;

    public SerilogLoggerConfigurationSpecimenBuilder()
    {
        _loggerConfigurationSpecification = new ExactTypeSpecification(typeof(LoggerConfiguration));
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (request is SeededRequest seededRequest && 
            _loggerConfigurationSpecification.IsSatisfiedBy(seededRequest.Request))
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.InMemory()
                .WriteTo.Debug();
        }

        return new NoSpecimen();
    }
}
