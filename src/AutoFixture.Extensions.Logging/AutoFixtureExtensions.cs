using AutoFixture.Extensions.Logging.Builders;
using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace AutoFixture.Extensions.Logging;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Registers a Microsoft Extensions ILogger&lt;T&gt; with the fixture, using the FakeLoggerProvider. <br/>
    /// Only allows creation of a logger matching the exact category type that is registered. <br/>
    /// Allows your tests to access the FakeLoggerProvider via: <br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&lt;SystemUnderTest&gt;&gt;();
    ///
    /// // perform test actions ...
    ///
    /// var logs = logger.Collector.GetSnapshot();
    /// </code>
    /// </summary>
    /// <returns>IFixture</returns>
    public static IFixture WithMicrosoftLogger<T>(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(ILogger<T>), typeof(FakeLogger<T>)));
        fixture.Register(() => new FakeLogger<T>());

        return fixture;
    }

    /// <summary>
    /// Registers a LoggerFactory, configured using the FakeLoggerProvider, with the fixture. <br/>
    /// Allows creation of loggers for any category. <br/>
    /// Allows your tests to access the LoggerFactory via: <br/><br/>
    /// <code>
    /// var loggerFactory = fixture.Freeze&lt;ILoggerFactory&gt;();
    /// </code>
    /// Or create ILogger instances, by having the registered LoggerFactory create them
    /// <code>
    /// var logger1 = fixture.Freeze&lt;ILogger&lt;SystemUnderTest1&gt;&gt;();
    /// var logger2 = fixture.Freeze&lt;ILogger&lt;SystemUnderTest2&gt;&gt;();
    /// </code>
    /// </summary>
    /// <returns>IFixture</returns>
    public static IFixture WithMicrosoftLoggerFactory(this IFixture fixture)
    {
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        fixture.Customizations.Add(new LoggerSpecimenBuilder());
        return fixture;
    }

    private static FilteringSpecimenBuilder CreateLoggerFactoryCustomization() =>
        new(new LoggerFactorySpecimenBuilder(), new OrRequestSpecification(new ExactTypeSpecification(typeof(ILoggerFactory)), new ExactTypeSpecification(typeof(LoggerFactory))));

}
