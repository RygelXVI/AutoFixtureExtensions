using AutoFixture;

namespace Jouska.AutoFixture.Extensions.Common.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_register_a_named_parameter_value()
    {
        var expected = "blah blah blah";
        var fixture = new Fixture().WithNamedParameterValue<string>("input", expected);

        var result = fixture.Create<TestSubject>();

        Assert.Multiple(
            () => Assert.Equal(expected, result.Input),
            () => Assert.NotEqual(expected, result.Input2)
        );
    }

    [Fact]
    public void Can_register_generic_type_relay()
    {
        var fixture = new Fixture().WithTypeRelay<IThing, Thing>();

        var thing = fixture.Freeze<IThing>();

        Assert.IsType<Thing>(thing);
    }

    [Fact]
    public void Can_inject_instance_with_interface_registration()
    {
        var expected = "test name";
        var thing = new Thing { Name = expected };

        var fixture = new Fixture().Inject<IThing, Thing>(thing);

        var result1 = fixture.Create<IThing>();
        var result2 = fixture.Create<Thing>();

        Assert.Multiple(
            () => Assert.Equal(expected, result1.Name),
            () => Assert.Equal(expected, result2.Name)
        );
    }
}

public class TestSubject
{
    public TestSubject(string input, string input2)
    {
        Input = input;
        Input2 = input2;
    }

    public string Input { get; }
    public string Input2 { get; }
}

public interface IThing
{
    string Name { get; }
}

public class Thing : IThing
{
    public string Name { get; set; } = string.Empty;
}