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