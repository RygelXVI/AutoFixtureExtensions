using AutoFixture;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Jouska.AutoFixture.Extensions.Serilog.Specification;

public class Specification
{
    [Fact]
    public void Can_create_logger_with_category()
    {
        var fixture = new Fixture().WithSerilogProvider();

        var ex1 = fixture.Create<TestExample1>();
        var ex2 = fixture.Create<TestExample2>();

        Assert.Multiple(
            () => Assert.NotNull(ex1),
            () => Assert.IsType<ILogger<TestExample1>>(ex1.Logger, exactMatch: false),
            () => Assert.NotNull(ex2),
            () => Assert.IsType<ILogger<TestExample2>>(ex2.Logger, exactMatch: false)
        );
    }

    [Fact]
    public void Can_create_logger_with_external_logger_configuration()
    {
        var externalConfig = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Verbose()
                    .WriteTo.Debug();                    

        var fixture = new Fixture().WithSerilogProvider(externalConfig);

        var ex1 = fixture.Create<TestExample1>();
        var ex2 = fixture.Create<TestExample2>();

        Assert.Multiple(
            () => Assert.NotNull(ex1),
            () => Assert.IsType<ILogger<TestExample1>>(ex1.Logger, exactMatch: false),
            () => Assert.True(((ILogger<TestExample1>)ex1.Logger).IsEnabled(LogLevel.Trace)),
            () => Assert.NotNull(ex2),
            () => Assert.IsType<ILogger<TestExample2>>(ex2.Logger, exactMatch: false)
        );
    }

    [Fact]
    public void Can_create_logger_with_externally_created_serilog_logger()
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
            () => Assert.IsType<ILogger<TestExample1>>(ex1.Logger, exactMatch: false),
            () => Assert.True(((ILogger<TestExample1>)ex1.Logger).IsEnabled(LogLevel.Trace)),
            () => Assert.NotNull(ex2),
            () => Assert.IsType<ILogger<TestExample2>>(ex2.Logger, exactMatch: false)
        );
    }



    public class TestExample1
    {
        public TestExample1(ILogger<TestExample1> logger)
        {
            Logger = logger;
        }

        public object Logger { get; }
    }

    public class TestExample2
    {
        public TestExample2(ILogger<TestExample2> logger)
        {
            Logger = logger;
        }

        public object Logger { get; }
    }
}

