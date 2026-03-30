using Jouska.AutoFixture.Extensions.Serilog.Builders;
using AutoFixture.Kernel;
using NSubstitute;
using Serilog;

namespace Jouska.AutoFixture.Extensions.Serilog.Specification;

public class SerilogLoggerConfigurationSpecimenBuilderSpecification
{
    [Fact]
    public void Can_create_logger_configuration_from_type_request()
    {
        var sut = new SerilogLoggerConfigurationSpecimenBuilder();
        var context = Substitute.For<ISpecimenContext>();

        var result = sut.Create(typeof(LoggerConfiguration), context);

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<LoggerConfiguration>(result)
        );
    }

    [Fact]
    public void Can_create_logger_configuration_from_seeded_request()
    {
        var sut = new SerilogLoggerConfigurationSpecimenBuilder();
        var context = Substitute.For<ISpecimenContext>();
        var seededRequest = new SeededRequest(typeof(LoggerConfiguration), null);

        var result = sut.Create(seededRequest, context);

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<LoggerConfiguration>(result)
        );
    }
}
