using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace AutoFixture.Extensions.Logging;

public static class AutoFixtureExtensions
{
    public static IFixture WithMicrosoftLogger<T>(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(ILogger<T>), typeof(FakeLogger<T>)));
        fixture.Register(() => new FakeLogger<T>());

        return fixture;
    }

    public static IFixture WithMicrosoftLoggerFactory(this IFixture fixture)
    {
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        return fixture;
    }

    private static FilteringSpecimenBuilder CreateLoggerFactoryCustomization() =>
        new(new LoggerFactorySpecimenBuilder(), new OrRequestSpecification(new ExactTypeSpecification(typeof(ILoggerFactory)), new ExactTypeSpecification(typeof(LoggerFactory))));

}

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