using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Jouska.AutoFixture.Extensions.Logging.Specification;

public class AutoFixtureExtensionsSpecification
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
    public void Can_register_all_loggers_as_fake_logger()
    {
        var fixture = new Fixture().WithMicrosoftLogging();

        var logger1 = fixture.Freeze<ILogger<TestClass1>>();

        Assert.IsType<ILogger<TestClass1>>(logger1, exactMatch: false);

        var sub = fixture.Freeze<LoggerTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(sub),
            () => Assert.IsType<ILogger<LoggerTestSubject>>(sub.Logger, exactMatch: false),
            () => Assert.IsType<FakeLogger<LoggerTestSubject>>(sub.Logger, exactMatch: true)
        );

        var logger2 = fixture.Freeze<ILogger<TestClass2>>();

        Assert.Multiple(
            () => Assert.NotNull(logger2),
            () => Assert.IsType<ILogger<TestClass2>>(logger2, exactMatch: false),
            () => Assert.IsType<FakeLogger<TestClass2>>(logger2, exactMatch: true)
        );
    }

    [Fact]
    public void Can_register_a_single_fake_logger()
    {
        var fixture = new Fixture().WithMicrosoftLogger<LoggerTestSubject>();

        var subject = fixture.Freeze<LoggerTestSubject>();

        Assert.NotNull(subject);

        try
        {
            var negativeSubject = fixture.Freeze<LoggerTestSubject2>();
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

        var testClass3 = fixture.Create<LoggerFactoryTestSubject>();

        var fakeLogger = fixture.GetFakeLoggerFromFactory<LoggerFactoryTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(fakeLogger),
            () => Assert.Equal(1, fakeLogger!.Collector.Count),
            () => Assert.Equal("this better be recorded", fakeLogger!.Collector.GetSnapshot().Single().Message)
        );
    }

    [Fact]
    public void Can_register_null_logger()
    {
        var fixture = new Fixture().WithNullLogging();

        var subject = fixture.Freeze<LoggerTestSubject>();

        Assert.NotNull(subject);
    }

    [Fact]
    public void Can_register_single_null_logger()
    {
        var fixture = new Fixture().WithNullLogger<LoggerTestSubject>();
        var subject = fixture.Freeze<LoggerTestSubject>();
        Assert.NotNull(subject);
        try
        {
            var negativeSubject = fixture.Freeze<LoggerTestSubject2>();
        }
        catch (Exception ex)
        {
            Assert.IsType<ObjectCreationException>(ex, exactMatch: false);
            return;
        }
        Assert.Fail();
    }

    [Fact]
    public void Can_register_null_logger_factory()
    {
        var fixture = new Fixture().WithNullLoggerFactory();
        var testClass3 = fixture.Create<LoggerFactoryTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(testClass3),
            () => Assert.All([LogLevel.Critical,LogLevel.Debug,LogLevel.Error, LogLevel.Information, LogLevel.None, LogLevel.Trace,LogLevel.Warning], level => Assert.False(testClass3.Logger.IsEnabled(level)))
        );
    }
}

public class TestClass1
{

}

public class TestClass2
{

}

public class LoggerFactoryTestSubject
{
    public LoggerFactoryTestSubject(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory?.CreateLogger<LoggerFactoryTestSubject>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        Logger.LogInformation("TestClass3 created");
        Logger.LogError("this better be recorded");
    }

        public ILogger<LoggerFactoryTestSubject> Logger { get; }
    }

public class LoggerTestSubject
{
    public LoggerTestSubject(ILogger<LoggerTestSubject> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ILogger<LoggerTestSubject> Logger { get; }
}

public class LoggerTestSubject2
{
    public LoggerTestSubject2(ILogger<LoggerTestSubject2> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ILogger<LoggerTestSubject2> Logger { get; }
}