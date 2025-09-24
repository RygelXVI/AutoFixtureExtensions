using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;

namespace AutoFixture.Extensions.Logging.Builders;

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
        if (request is SeededRequest seededRequest)
        {
            if (_genericLoggerSpecification.IsSatisfiedBy(seededRequest.Request))
            {
                var loggerFactoryExtensionsType = typeof(LoggerFactoryExtensions);
                var createLoggerMethodInfo = loggerFactoryExtensionsType.GetMethods().FirstOrDefault(x => x.IsGenericMethod);

                if (createLoggerMethodInfo != null)
                {
                    var innerRequest = (Type)seededRequest.Request;
                    var categoryType = innerRequest.UnderlyingSystemType.GetGenericArguments()[0];
                    var genericMethod = createLoggerMethodInfo.MakeGenericMethod(categoryType);
                    var factory = context.Create<ILoggerFactory>();

                    var logger = genericMethod.Invoke(null, [factory]);

                    if (logger != null)
                    {
                        return logger;
                    }
                }
            }

            if (_loggerSpecification.IsSatisfiedBy(seededRequest.Request))
            {
                var factory = context.Create<ILoggerFactory>();
                var logger = factory.CreateLogger("default");
                return logger;
            }
        }

        return new NoSpecimen();
    }
}
