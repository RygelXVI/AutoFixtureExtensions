using AutoFixture;
using Imposter.Abstractions;
using Jouska.AutoFixture.AutoImposter.TestHelpers;
using Xunit;

namespace Jouska.AutoFixture.AutoImposter.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_create_imposter_instance()
    {
        var fixture = new Fixture().WithAutoImposter(typeof(AutoFixtureExtensionsSpecification).Assembly);

        var calculator = fixture.Create<ICalculator>();

        Assert.NotNull(calculator);
        Assert.IsType<ICalculator>(calculator, exactMatch: false);    
        
        var result = calculator.Add(1, 2);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Can_freeze_imposter()
    {
        var fixture = new Fixture();

        var imposter = fixture.Freeze<ICalculatorImposter>();
        imposter
            .Add(Arg<int>.Any(), Arg<int>.Any())
            .Returns(42);

        var imposterInstance = fixture.FreezeImposter<ICalculator, ICalculatorImposter>();

        Assert.NotNull(imposterInstance);
        Assert.IsType<ICalculator>(imposterInstance, exactMatch: false);

        var actual = imposterInstance.Add(1, 2);

        Assert.Equal(42, actual);
    }

    [Fact]
    public void Can_inject_imposter()
    {
        var fixture = new Fixture();

        var imposter = new ICalculatorImposter();
        imposter
            .Add(Arg<int>.Any(), Arg<int>.Any())
            .Returns(42);
        
        var imposterInstance = fixture.FreezeImposter<ICalculator, ICalculatorImposter>(imposter);
        
        Assert.NotNull(imposterInstance);
        Assert.IsType<ICalculator>(imposterInstance, exactMatch: false);
        var actual = imposterInstance.Add(1, 2);
        Assert.Equal(42, actual);
    }
}
