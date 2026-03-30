using AutoFixture.Kernel;
using Serilog;
using Serilog.Sinks.InMemory;

namespace Jouska.AutoFixture.Extensions.Serilog.Builders;

public class SerilogLoggerConfigurationSpecimenBuilder : ISpecimenBuilder
{
    private readonly ExactTypeSpecification _loggerConfigurationSpecification;

    public SerilogLoggerConfigurationSpecimenBuilder()
    {
        _loggerConfigurationSpecification = new ExactTypeSpecification(typeof(LoggerConfiguration));
    }

    public object Create(object request, ISpecimenContext context)
    {
        var requestType = TryGetRequestType(request);

        return requestType switch
        {
            _ when _loggerConfigurationSpecification.IsSatisfiedBy(requestType) => CreateLoggerConfiguration(),
            _ => new NoSpecimen()
        };
    }

    private static Type? TryGetRequestType(object request)
    {
        if (request is Type typeRequest)
        {
            return typeRequest;
        }

        if (request is SeededRequest seededRequest && seededRequest.Request is Type seededTypeRequest)
        {
            return seededTypeRequest;
        }

        return null;
    }

    private static LoggerConfiguration CreateLoggerConfiguration() => 
        new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.InMemory()
            .WriteTo.Debug();
}
