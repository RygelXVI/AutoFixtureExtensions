using AutoFixture;
using Jouska.AutoFixture.Extensions.Serilog.Builders;
using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using ILogger = Serilog.ILogger;

namespace Jouska.AutoFixture.Extensions.Serilog;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Registers an ILoggerFactory, configured with the SerilogProvider. <br/>
    /// Allows use of Microsoft Extensions Logging loggers, with logs accessed from the Serilog InMemory sink.<br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&lt;SystemUnderTest&gt;&gt;;
    /// 
    /// // perform test actions...
    /// 
    /// var logs = InMemorySink.Instance.Snapshot();
    /// </code>
    /// </summary>
    /// <returns>IFixture</returns>
    public static IFixture WithSerilogProvider(this IFixture fixture)
    {
        fixture.Customizations.Add(CreateSerilogLoggerCustomization());
        fixture.Customizations.Add(CreateSerilogTypeRelay());
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        fixture.Customizations.Add(new LoggerSpecimenBuilder());

        return fixture;
    }

    /// <summary>
    /// Registers an ILoggerFactory, configured with the SerilogProvider using the provided LoggerConfiguration. <br/>
    /// Allows use of Microsoft Extensions Logging loggers, with logs accessed from the Serilog InMemory sink.<br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&lt;SystemUnderTest&gt;&gt;;
    /// 
    /// // perform test actions...
    /// 
    /// var logs = InMemorySink.Instance.Snapshot();
    /// </code>
    /// </summary>
    /// <param name="loggerConfiguration">serilog configuration to be used by the SerilogProvider in the LoggerFactory</param>
    /// <returns>IFixture</returns>
    public static IFixture WithSerilogProvider(this IFixture fixture, LoggerConfiguration loggerConfiguration)
    {
        var serilogLogger = loggerConfiguration.CreateLogger();
        fixture.Register(() => serilogLogger);
        fixture.Customizations.Add(CreateSerilogTypeRelay());
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        fixture.Customizations.Add(new LoggerSpecimenBuilder());

        return fixture;
    }

    /// <summary>
    /// Registers an ILoggerFactory, configured with the SerilogProvider using the provided Serilog logger instance. <br/>
    /// Allows use of Microsoft Extensions Logging loggers, with logs accessed from the Serilog InMemory sink.<br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&lt;SystemUnderTest&gt;&gt;;
    /// 
    /// // perform test actions...
    /// 
    /// var logs = InMemorySink.Instance.Snapshot();
    /// </code>
    /// </summary>
    /// <param name="logger">serilog logger instance to be used by the SerilogProvider in the LoggerFactory</param>
    /// <returns>IFixture</returns>
    public static IFixture WithSerilogProvider(this IFixture fixture, Logger logger)
    {
        fixture.Register(() => logger);
        fixture.Customizations.Add(CreateSerilogTypeRelay());
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        fixture.Customizations.Add(new LoggerSpecimenBuilder());

        return fixture;
    }

    /// <summary>
    /// Registers a Serilog logger using a default configuration. <br/>
    /// Allows tests to access the logger using Serilog interfaces, and allows logs to be accessed from the Serilog InMemory sink.<br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&gt;;
    /// 
    /// // perform test actions...
    /// 
    /// var logs = InMemorySink.Instance.Snapshot();
    /// </code>
    /// </summary>
    /// <returns>IFixture</returns>
    public static IFixture WithSerilog(this IFixture fixture)
    {
        fixture.Register(() => new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .WriteTo.InMemory()
            .CreateLogger());

        fixture.Customizations.Add(CreateSerilogTypeRelay());

        return fixture;
    }

    /// <summary>
    /// Registers a Serilog logger using a default configuration and the provided minimum log level. <br/>
    /// Allows tests to access the logger using Serilog interfaces, and allows logs to be accessed from the Serilog InMemory sink.<br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&gt;;
    /// 
    /// // perform test actions...
    /// 
    /// var logs = InMemorySink.Instance.Snapshot();
    /// </code>
    /// </summary>
    /// <param name="minimumLogEventLevel">minimum log level for the logger configuration</param>
    /// <returns>IFixture</returns>
    public static IFixture WithSerilog(this IFixture fixture, LogEventLevel minimumLogEventLevel)
    {
        fixture.Register(() => new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Is(minimumLogEventLevel)
            .WriteTo.Debug()
            .WriteTo.InMemory()
            .CreateLogger());

        fixture.Customizations.Add(CreateSerilogTypeRelay());

        return fixture;
    }

    /// <summary>
    /// Registers a Serilog logger using the provided logger configuration. <br/>
    /// Allows tests to access the logger using Serilog interfaces, and allows logs to be accessed from the Serilog InMemory sink.<br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&gt;;
    /// 
    /// // perform test actions...
    /// 
    /// var logs = InMemorySink.Instance.Snapshot();
    /// </code>
    /// </summary>
    /// <param name="loggerConfiguration">configuration for serilog logger</param>
    /// <returns>IFixture</returns>
    public static IFixture WithSerilog(this IFixture fixture, LoggerConfiguration loggerConfiguration)
    {
        fixture.Register(() => loggerConfiguration.CreateLogger());
        fixture.Customizations.Add(CreateSerilogTypeRelay());

        return fixture;
    }

    /// <summary>
    /// Registers a Serilog logger using the provided Serilog logger instance. <br/>
    /// Allows tests to access the logger using Serilog interfaces, and allows logs to be accessed from the Serilog InMemory sink.<br/><br/>
    /// <code>
    /// var logger = fixture.Freeze&lt;ILogger&gt;;
    /// 
    /// // perform test actions...
    /// 
    /// var logs = InMemorySink.Instance.Snapshot();
    /// </code>
    /// </summary>
    /// <param name="logger">the Serilog logger instance</param>
    /// <returns>IFixture</returns>
    public static IFixture WithSerilog(this IFixture fixture, Logger logger)
    {
        fixture.Register(() => logger);
        fixture.Customizations.Add(CreateSerilogTypeRelay());

        return fixture;
    }

    private static FilteringSpecimenBuilder CreateLoggerFactoryCustomization() =>
        new(new LoggerFactorySpecimenBuilder(), new OrRequestSpecification(new ExactTypeSpecification(typeof(ILoggerFactory)), new ExactTypeSpecification(typeof(LoggerFactory))));

    private static FilteringSpecimenBuilder CreateSerilogLoggerCustomization() =>
        new(new SerilogLoggerSpecimenBuilder(), new OrRequestSpecification(new ExactTypeSpecification(typeof(Logger)), new ExactTypeSpecification(typeof(ILogger))));

    private static TypeRelay CreateSerilogTypeRelay() =>
        new(typeof(ILogger), typeof(Logger));
}
