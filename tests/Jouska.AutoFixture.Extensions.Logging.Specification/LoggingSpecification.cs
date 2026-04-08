using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Jouska.AutoFixture.Extensions.Logging.Specification;

public class LoggingSpecification
{
    [Fact]
    public void Can_create_loggers_with_logger_factory()
    {
        var fixture = new Fixture().WithMicrosoftLoggerFactory();

        var logger1 = fixture.Freeze<ILogger<TestClass1>>();
        var logger2 = fixture.Freeze<ILogger<TestClass2>>();

        Assert.Multiple(
            () => Assert.IsType<ILogger<TestClass1>>(logger1, exactMatch: false),
            () => Assert.IsType<ILogger<TestClass2>>(logger2, exactMatch: false)
        );
    }

    [Fact]
    public void Can_create_single_logger_instance()
    {
        var fixture = new Fixture().WithMicrosoftLogger<TestClass1>();

        var logger1 = fixture.Freeze<ILogger<TestClass1>>();

        Assert.IsType<ILogger<TestClass1>>(logger1, exactMatch: false);

        try
        {
            var logger2 = fixture.Freeze<ILogger<TestClass2>>();
        }
        catch (Exception ex)
        {
            Assert.IsType<ObjectCreationException>(ex, exactMatch: false);
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_configure_log_collector_options()
    {
        var fixture = new Fixture()
            .WithMicrosoftLogger<TestClass1>(options =>
            {
                options.CollectRecordsForDisabledLogLevels = false;
                options.FilteredLevels = new HashSet<LogLevel> { LogLevel.Error };
            });

        var logger = fixture.Freeze<ILogger<TestClass1>>();

        logger.LogInformation("this should be ignored");
        logger.LogError("this should be recorded");

        var fakeLogger = logger as FakeLogger<TestClass1>;

        Assert.Equal(1, fakeLogger!.Collector.Count);
    }

    [Fact]
    public void Can_configure_log_collector_options_with_factory()
    {
        var fixture = new Fixture()
            .WithMicrosoftLoggerFactory(options =>
            {
                options.CollectRecordsForDisabledLogLevels = false;
                options.FilteredLevels = new HashSet<LogLevel> { LogLevel.Error };
            });

        var testClass3 = fixture.Create<TestClass3>();

        var fakeLogger = fixture.GetFakeLoggerFromFactory<TestClass3>();

        Assert.Multiple(
            () => Assert.NotNull(fakeLogger),
            () => Assert.Equal(1, fakeLogger!.Collector.Count),
            () => Assert.Equal("this better be recorded", fakeLogger!.Collector.GetSnapshot().Single().Message)
        );
    }
}

public class TestClass1
{

}

public class TestClass2
{

}

public class TestClass3
{
    private readonly ILogger<TestClass3> _logger;

    public TestClass3(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TestClass3>();
        _logger.LogInformation("TestClass3 created");
        _logger.LogError("this better be recorded");
    }
}