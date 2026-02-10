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
    /// Configures the fixture to use a Microsoft.Extensions.Logging logger for the specified type, allowing
    /// customization of log collection options.
    /// </summary>
    /// <remarks>This method enables advanced log collection scenarios by allowing customization of the
    /// underlying log collector options before the logger is added to the fixture. The logger will be available for use
    /// in tests targeting the specified type.</remarks>
    /// <typeparam name="T">The type for which the logger will be created and associated within the fixture.</typeparam>
    /// <param name="configureOptions">A delegate that configures the options for collecting and handling log messages. Cannot be null.</param>
    /// <returns>IFixture</returns>
    public static IFixture WithMicrosoftLogger<T>(this IFixture fixture, Action<FakeLogCollectorOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(configureOptions);

        var options = new FakeLogCollectorOptions();
        configureOptions(options);
        return fixture.WithMicrosoftLogger<T>(options);
    }

    /// <summary>
    /// Configures the specified fixture to use a fake Microsoft ILogger implementation for type T, enabling log
    /// collection during testing.
    /// </summary>
    /// <remarks>After calling this method, requests for ILogger or ILogger<T> from the fixture will return a
    /// fake logger that collects log entries for inspection. This is useful for verifying logging behavior in unit
    /// tests.</remarks>
    /// <typeparam name="T">The type for which the fake logger will be created. Typically, this is the class under test.</typeparam>
    /// <param name="options">The options used to configure the fake log collector. Cannot be null.</param>
    /// <returns>IFixture</returns>
    public static IFixture WithMicrosoftLogger<T>(this IFixture fixture, FakeLogCollectorOptions options)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(ILogger), typeof(FakeLogger)));
        fixture.Register(() => new FakeLogger<T>(FakeLogCollector.Create(options)));
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

    /// <summary>
    /// Configures the specified fixture to use a Microsoft-compatible logger factory and enables collection of log
    /// messages for testing purposes.
    /// </summary>
    /// <remarks>This extension method adds customizations to the fixture that allow components requiring
    /// ILoggerFactory or ILogger to be resolved, and enables capturing log output for assertions in tests. Thread
    /// safety depends on the underlying fixture implementation.</remarks>
    /// <param name="fixture">The fixture to be customized with Microsoft logger factory support. Cannot be null.</param>
    /// <param name="configureOptions">An action to configure options for collecting log messages. Cannot be null.</param>
    /// <returns>The same fixture instance, configured to provide Microsoft logger factory and log collection capabilities.</returns>
    public static IFixture WithMicrosoftLoggerFactory(this IFixture fixture, Action<FakeLogCollectorOptions> configureOptions)
    {
        fixture.Customizations.Add(CreateLoggerFactoryCustomization(configureOptions));
        fixture.Customizations.Add(new LoggerSpecimenBuilder());
        return fixture;
    }

    private static FilteringSpecimenBuilder CreateLoggerFactoryCustomization(Action<FakeLogCollectorOptions> configureOptions) =>
        new(new LoggerFactorySpecimenBuilder(configureOptions), new OrRequestSpecification(new ExactTypeSpecification(typeof(ILoggerFactory)), new ExactTypeSpecification(typeof(LoggerFactory))));

    private static FilteringSpecimenBuilder CreateLoggerFactoryCustomization() =>
        new(new LoggerFactorySpecimenBuilder(), new OrRequestSpecification(new ExactTypeSpecification(typeof(ILoggerFactory)), new ExactTypeSpecification(typeof(LoggerFactory))));

}
