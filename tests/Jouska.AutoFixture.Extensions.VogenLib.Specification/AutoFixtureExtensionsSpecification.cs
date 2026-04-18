using AutoFixture;

namespace Jouska.AutoFixture.Extensions.VogenLib.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_build_vogen_value_object()
    {
        var fixture = new Fixture().WithDefaultVogenBuilder();

        var actual = fixture.Create<TestClass>();

        Assert.Multiple(
            () => Assert.NotNull(actual),
            () => Assert.IsType<TestClass>(actual),
            () => Assert.IsType<int>(actual.Value)
        );
    }

    [Fact]
    public void Can_build_type_with_vogen_value_object_dependency()
    {
        var fixture = new Fixture().WithDefaultVogenBuilder();

        var actual = fixture.Freeze<TestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(actual),
            () => Assert.IsType<TestSubject>(actual),
            () => Assert.IsType<TestClass>(actual.TestClass),
            () => Assert.IsType<int>(actual.TestClass.Value)
        );
    }
}

public class TestSubject
{
    public TestSubject(TestClass testClass)
    {
        TestClass = testClass ?? throw new ArgumentNullException(nameof(testClass));
    }

    public TestClass TestClass { get; }
}