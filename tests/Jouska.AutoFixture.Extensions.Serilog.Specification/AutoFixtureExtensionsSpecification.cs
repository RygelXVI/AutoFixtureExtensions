using AutoFixture;
using MEL = Microsoft.Extensions.Logging;
using Serilog;
using SC = Serilog.Core;
using SE = Serilog.Events;

namespace Jouska.AutoFixture.Extensions.Serilog.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_create_microsoft_logger_with_serilog_provider()
    {
        var fixture = new Fixture().WithSerilogProvider();

        var ex1 = fixture.Create<TestExample1>();
        var ex2 = fixture.Create<TestExample2>();

        Assert.Multiple(
            () => Assert.NotNull(ex1),
            () => Assert.IsType<MEL.ILogger<TestExample1>>(ex1.Logger, exactMatch: false),
            () => Assert.NotNull(ex2),
            () => Assert.IsType<MEL.ILogger<TestExample2>>(ex2.Logger, exactMatch: false)
        );
    }

    [Fact]
    public void Can_create_microsoft_logger_with_externally_configured_serilog_provider()
    {
        var fixture = new Fixture()
            .WithSerilogProvider(x => x
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.Debug()
            );

        var ex1 = fixture.Create<TestExample1>();
        var ex2 = fixture.Create<TestExample2>();

        Assert.Multiple(
            () => Assert.NotNull(ex1),
            () => Assert.IsType<MEL.ILogger<TestExample1>>(ex1.Logger, exactMatch: false),
            () => Assert.True(((MEL.ILogger<TestExample1>)ex1.Logger).IsEnabled(MEL.LogLevel.Trace)),
            () => Assert.NotNull(ex2),
            () => Assert.IsType<MEL.ILogger<TestExample2>>(ex2.Logger, exactMatch: false)
        );
    }

    [Fact]
    public void Can_create_microsoft_logger_with_externally_provided_serilog_logger()
    {
        var serilogLogger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Debug()
            .CreateLogger();
        
        var fixture = new Fixture().WithSerilogProvider(serilogLogger);
        
        var ex1 = fixture.Create<TestExample1>();
        var ex2 = fixture.Create<TestExample2>();

        Assert.Multiple(
            () => Assert.NotNull(ex1),
            () => Assert.IsType<MEL.ILogger<TestExample1>>(ex1.Logger, exactMatch: false),
            () => Assert.True(((MEL.ILogger<TestExample1>)ex1.Logger).IsEnabled(MEL.LogLevel.Trace)),
            () => Assert.NotNull(ex2),
            () => Assert.IsType<MEL.ILogger<TestExample2>>(ex2.Logger, exactMatch: false)
        );
    }

    [Fact]
    public void Can_create_serilog_logger_with_default_configuration()
    {
        var fixture = new Fixture().WithSerilog();
        var logger = fixture.Create<ILogger>();

        Assert.Multiple(
            () => Assert.NotNull(logger),
            () => Assert.IsType<SC.Logger>(logger, exactMatch: false)
        );
    }

    [Fact]
    public void Can_create_serilog_logger_with_configured_logging_level()
    {
        var fixture = new Fixture().WithSerilog(SE.LogEventLevel.Debug);

        var logger = fixture.Create<ILogger>();

        Assert.Multiple(
            () => Assert.NotNull(logger),
            () => Assert.True(logger.IsEnabled(SE.LogEventLevel.Debug))
        );
    }

    [Fact]
    public void Can_create_serilog_logger_with_external_configuration()
    {
        var fixture = new Fixture()
            .WithSerilog(x => x
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.Debug());
        
        var logger = fixture.Create<ILogger>();

        Assert.Multiple(
            () => Assert.NotNull(logger),
            () => Assert.IsType<SC.Logger>(logger, exactMatch: false),
            () => Assert.True(logger.IsEnabled(SE.LogEventLevel.Debug))
        );
    }

    [Fact]
    public void Can_inject_serilog_logger()
    {
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Debug()
            .CreateLogger();

        var fixture = new Fixture().WithSerilog(logger);

        var example = fixture.Create<TestExample3>();

        Assert.Multiple(
            () => Assert.NotNull(example.Logger),
            () => Assert.IsType<SC.Logger>(example.Logger, exactMatch: false),
            () => Assert.True(example.Logger.IsEnabled(SE.LogEventLevel.Debug))
        );
    }


    public class TestExample1
    {
        public TestExample1(MEL.ILogger<TestExample1> logger)
        {
            Logger = logger;
        }

        public object Logger { get; }
    }

    public class TestExample2
    {
        public TestExample2(MEL.ILogger<TestExample2> logger)
        {
            Logger = logger;
        }

        public object Logger { get; }
    }

    public class TestExample3
    {
        public TestExample3(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }
    }
}

