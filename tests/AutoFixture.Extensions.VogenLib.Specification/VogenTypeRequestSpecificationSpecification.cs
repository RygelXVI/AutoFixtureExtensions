
using AutoFixture.Extensions.VogenLib.Builders;

namespace AutoFixture.Extensions.VogenLib.Specification;

public class VogenTypeRequestSpecificationSpecification
{
    [Fact]
    public void Vogen_type_specification_can_find_generic_value_object_attribute()
    {
        var input = TestClass.From(1);

        var sut = new VogenTypeSpecification();

        var actual = sut.IsSatisfiedBy(input);

        Assert.True(actual);
    }

    [Fact]
    public void Vogen_type_specification_can_find_value_object_attribute()
    {
        var input = TestClass2.From(1);

        var sut = new VogenTypeSpecification();

        var actual = sut.IsSatisfiedBy(input);

        Assert.True(actual);
    }

    [Fact]
    public void Can_build_vogen_value_object()
    {
        var fixture = new Fixture().WithDefaultVogenBuilder();

        var actual = fixture.Create<TestClass>();

        Assert.Multiple(
            () => Assert.NotNull(actual),
            () => Assert.IsType<TestClass>(actual));
    }
}
