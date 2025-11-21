
using AutoFixture.Extensions.VogenLib.Builders;
using AutoFixture.Kernel;

namespace AutoFixture.Extensions.VogenLib.Specification;

public class VogenTypeRequestSpecificationSpecification
{
    [Fact]
    public void Vogen_type_specification_can_find_generic_value_object_attribute()
    {
        var seededRequest = new SeededRequest(typeof(TestClass), 1);

        var sut = new VogenTypeSpecification();

        var actual = sut.IsSatisfiedBy(seededRequest);

        Assert.True(actual);
    }

    [Fact]
    public void Vogen_type_specification_can_find_value_object_attribute()
    {
        var seededRequest = new SeededRequest(typeof(TestClass2), 1);
        var sut = new VogenTypeSpecification();

        var actual = sut.IsSatisfiedBy(seededRequest);

        Assert.True(actual);
    }

    [Fact]
    public void Can_build_vogen_value_object()
    {
        var fixture = new Fixture().WithDefaultVogenBuilder();

        var actual = fixture.Create<TestClass>();

        Assert.Multiple(
            () => Assert.NotNull(actual),
            () => Assert.IsType<TestClass>(actual),
            () => Assert.IsType<int>(actual.Value));
    }
}
