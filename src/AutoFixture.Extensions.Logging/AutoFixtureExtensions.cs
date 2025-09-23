using AutoFixture.Extensions.Logging.Builders;
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
