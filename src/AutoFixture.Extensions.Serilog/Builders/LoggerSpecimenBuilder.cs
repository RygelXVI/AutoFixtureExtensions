using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;

namespace AutoFixture.Extensions.Serilog.Builders;

public class LoggerSpecimenBuilder : ISpecimenBuilder
{
    private readonly ExactTypeSpecification _genericLoggerSpecification;
    private readonly ExactTypeSpecification _loggerSpecification;

    public LoggerSpecimenBuilder()
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
            _ when _genericLoggerSpecification.IsSatisfiedBy(type) => CreateGenericLogger(type, context),
            _ when _loggerSpecification.IsSatisfiedBy(type) => CreateDefaultLogger(context),
            _ => new NoSpecimen()
        };

    private static object CreateGenericLogger(Type type, ISpecimenContext context)
    {
        var loggerFactoryExtensionsType = typeof(LoggerFactoryExtensions);
        var createLoggerMethodInfo = loggerFactoryExtensionsType.GetMethods().FirstOrDefault(x => x.IsGenericMethod);

        if (createLoggerMethodInfo != null)
        {
            var categoryType = type.UnderlyingSystemType.GetGenericArguments()[0];
            var genericMethod = createLoggerMethodInfo.MakeGenericMethod(categoryType);
            var factory = context.Create<ILoggerFactory>();

            var logger = genericMethod.Invoke(null, [factory]);

            if (logger != null)
            {
                return logger;
            }
        }

        return new NoSpecimen();
    }

    private static ILogger CreateDefaultLogger(ISpecimenContext context)
    {
        var factory = context.Create<ILoggerFactory>();
        var logger = factory.CreateLogger("default");
        return logger;
    }
}
