using AutoFixture;
using AutoFixture.Kernel;
using Jouska.AutoFixture.Extensions.Common.Builders;

namespace Jouska.AutoFixture.Extensions.Common.Specification;

public class StaticFactoryMethodSpecimenBuilderSpecification
{
    [Fact]
    public void Can_create_target_using_static_factory_method()
    {
        var fixture = new Fixture();
        var builder = new StaticFactoryMethodSpecimenBuilder();
        fixture.Customizations.Add(new FilteringSpecimenBuilder(builder, new ExactTypeSpecification(typeof(TestExample1))));

        var result = fixture.Create<TestExample1>();

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<TestExample1>(result)
        );
    }

    [Fact]
    public void Can_create_target_using_method_with_most_parameters()
    {
        var fixture = new Fixture();
        var builder = new StaticFactoryMethodSpecimenBuilder();
        fixture.Customizations.Add(new FilteringSpecimenBuilder(builder, new ExactTypeSpecification(typeof(TestExample2))));
        var result = fixture.Create<TestExample2>();

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<TestExample2>(result),
            () => Assert.NotEqual("default", result.Note)
        );
    }
}

public class TestExample1
{
    private readonly int _value;
    private readonly string _note;

    private TestExample1(int value, string note)
    {
        _value = value;
        _note = note;
    }

    public static TestExample1 Create(int value, string note)
    {
        return new TestExample1(value, note);
    }
}

public class TestExample2
{
    private readonly int _value;

    private TestExample2(int value, string note)
    {
        _value = value;
        Note = note;
    }

    public string Note { get; }

    public static TestExample2 Create(int value)
    {
        return new TestExample2(value, "default");
    }

    public static TestExample2 Create(int value, string note)
    {
        return new TestExample2(value, note);
    }
}