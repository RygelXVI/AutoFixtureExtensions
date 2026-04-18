using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Jouska.AutoFixture.Extensions.Logging.Builders;

public class FakeLoggerSpecimenBuilder : ISpecimenBuilder
{
    private readonly ExactTypeSpecification _genericLoggerSpecification;
    private readonly ExactTypeSpecification _loggerSpecification;

    public FakeLoggerSpecimenBuilder()
    {
        _genericLoggerSpecification = new ExactTypeSpecification(typeof(ILogger<>)); 
        _loggerSpecification = new ExactTypeSpecification(typeof(ILogger));
    }

    public object Create(object request, ISpecimenContext context)
    {
        var requestType = TryGetRequestType(request);
        return requestType != null
            ? CreateLogger(requestType, context)
            : new NoSpecimen();
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

    private object CreateLogger(Type type, ISpecimenContext context) =>
        type switch
        {
            _ when _genericLoggerSpecification.IsSatisfiedBy (type) => CreateGenericLogger(type, context),
            _ when _loggerSpecification.IsSatisfiedBy(type) => CreateDefaultLogger(),
            _ => new NoSpecimen()
        };

    private static object CreateGenericLogger(Type type, ISpecimenContext context)
    {
        var categoryType = type.UnderlyingSystemType.GetGenericArguments()[0];
        var nullLoggerType = typeof(FakeLogger<>).MakeGenericType(categoryType);
        FakeLogCollector? collector = default;

        // use the concrete FakeLogCollector Type from the testing assembly
        var collectorType = typeof(FakeLogCollector);
        var ctor = nullLoggerType.GetConstructor([collectorType])
                   ?? throw new InvalidOperationException("Expected FakeLogCollector ctor not found.");

        // collector can be null; Invoke will call the correct ctor explicitly
        return ctor.Invoke([collector])!;
    }

    private static FakeLogger CreateDefaultLogger() => 
        new();
}