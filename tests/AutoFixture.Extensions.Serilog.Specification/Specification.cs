using Microsoft.Extensions.Logging;
using Serilog;

namespace AutoFixture.Extensions.Serilog.Specification;

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
            () => Assert.IsAssignableFrom<ILogger<TestExample1>>(ex1.Logger),
            () => Assert.NotNull(ex2),
            () => Assert.IsAssignableFrom<ILogger<TestExample2>>(ex2.Logger)
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
            () => Assert.IsAssignableFrom<ILogger<TestExample1>>(ex1.Logger),
            () => Assert.True(((ILogger<TestExample1>)ex1.Logger).IsEnabled(LogLevel.Trace)),
            () => Assert.NotNull(ex2),
            () => Assert.IsAssignableFrom<ILogger<TestExample2>>(ex2.Logger)
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
            () => Assert.IsAssignableFrom<ILogger<TestExample1>>(ex1.Logger),
            () => Assert.True(((ILogger<TestExample1>)ex1.Logger).IsEnabled(LogLevel.Trace)),
            () => Assert.NotNull(ex2),
            () => Assert.IsAssignableFrom<ILogger<TestExample2>>(ex2.Logger)
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

