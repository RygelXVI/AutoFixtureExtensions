using AutoFixture.Kernel;
using Jouska.AutoFixture.Extensions.VogenLib.Builders;

namespace Jouska.AutoFixture.Extensions.VogenLib.Specification;

public class VogenTypeRequestSpecificationSpecification
{
    [Fact]
    public void Vogen_type_specification_from_seeded_request_can_find_generic_value_object_attribute()
    {
        var seededRequest = new SeededRequest(typeof(TestClass), 1);

        var sut = new VogenTypeSpecification();

        var actual = sut.IsSatisfiedBy(seededRequest);

        Assert.True(actual);
    }

    [Fact]
    public void Vogen_type_specification_from_seeded_request_can_find_value_object_attribute()
    {
        var seededRequest = new SeededRequest(typeof(TestClass2), 1);
        var sut = new VogenTypeSpecification();

        var actual = sut.IsSatisfiedBy(seededRequest);

        Assert.True(actual);
    }

    [Fact]
    public void Vogen_type_specification_from_type_request_can_find_value_object_attribute()
    {
        var sut = new VogenTypeSpecification();
        var actual = sut.IsSatisfiedBy(typeof(TestClass));

        Assert.True(actual);
    }

    [Fact]
    public void Vogen_type_specification_from_type_request_can_find_generic_value_object_attribute()
    {
        var sut = new VogenTypeSpecification();
        var actual = sut.IsSatisfiedBy(typeof(TestClass2));

        Assert.True(actual);
    }

    [Fact]
    public void Vogen_type_specification_is_not_satisfied_by_non_vogen_type()
    {
        var sut = new VogenTypeSpecification();
        var actual = sut.IsSatisfiedBy(typeof(string));

        Assert.False(actual);
    }
}
