using Microsoft.Extensions.Logging;

namespace AutoFixture.Extensions.Logging.Specification;

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
}

public class TestClass1
{

}

public class TestClass2
{

}