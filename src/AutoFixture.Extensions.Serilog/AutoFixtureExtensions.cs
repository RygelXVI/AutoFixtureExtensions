using AutoFixture.Extensions.Serilog.Builders;
using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using ILogger = Serilog.ILogger;

namespace AutoFixture.Extensions.Serilog;

public static class AutoFixtureExtensions
{
    public static IFixture WithSerilogProvider(this IFixture fixture)
    {
        fixture.Customizations.Add(CreateSerilogLoggerCustomization());
        fixture.Customizations.Add(CreateSerilogTypeRelay());
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        fixture.Customizations.Add(new LoggerSpecimenBuilder());

        return fixture;
    }

    public static IFixture WithSerilogProvider(this IFixture fixture, LoggerConfiguration loggerConfiguration)
    {
        var serilogLogger = loggerConfiguration.CreateLogger();
        fixture.Register(() => serilogLogger);
        fixture.Customizations.Add(CreateSerilogTypeRelay());
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        fixture.Customizations.Add(new LoggerSpecimenBuilder());

        return fixture;
    }

    public static IFixture WithSerilogProvider(this IFixture fixture, Logger logger)
    {
        fixture.Register(() => logger);
        fixture.Customizations.Add(CreateSerilogTypeRelay());
        fixture.Customizations.Add(CreateLoggerFactoryCustomization());
        fixture.Customizations.Add(new LoggerSpecimenBuilder());

        return fixture;
    }

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

    public static IFixture WithSerilog(this IFixture fixture, LoggerConfiguration loggerConfiguration)
    {
        fixture.Register(() => loggerConfiguration.CreateLogger());
        fixture.Customizations.Add(CreateSerilogTypeRelay());

        return fixture;
    }

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
