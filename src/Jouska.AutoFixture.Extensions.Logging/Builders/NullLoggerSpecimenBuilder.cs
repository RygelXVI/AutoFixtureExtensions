using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Jouska.AutoFixture.Extensions.Logging.Builders;

public class NullLoggerSpecimenBuilder : ISpecimenBuilder
{
    private readonly ExactTypeSpecification _genericLoggerSpecification;

    public NullLoggerSpecimenBuilder()
    {
        _genericLoggerSpecification = new ExactTypeSpecification(typeof(ILogger<>));
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
        _ when _genericLoggerSpecification.IsSatisfiedBy(type) => CreateGenericLogger(type, context),
        _ => new NoSpecimen()
    };

    private static object CreateGenericLogger(Type type, ISpecimenContext context)
    {
        var categoryType = type.UnderlyingSystemType.GetGenericArguments()[0];
        var nullLoggerType = typeof(NullLogger<>).MakeGenericType(categoryType);
        return Activator.CreateInstance(nullLoggerType)!;
    }
}
