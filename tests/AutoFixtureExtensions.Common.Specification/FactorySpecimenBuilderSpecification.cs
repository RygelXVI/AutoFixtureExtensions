namespace AutoFixture.Extensions.Common.Specification;

public class FactorySpecimenBuilderSpecification
{
    [Fact]
    public void Can_construct_concrete_request_type_using_factory_to_create_exact_request_type()
    {
        var fixture = new Fixture().WithTypeConstructedByFactory<TestTarget, TestFactory>();

        var target = fixture.Create<TestTarget>();

        Assert.Multiple(
            () => Assert.NotNull(target),
            () => Assert.IsType<TestTarget>(target)
        );
    }

    [Fact]
    public void Can_construct_interface_request_type_using_factory_to_create_concrete_request_type()
    {
        var fixture = new Fixture().WithTypeConstructedByFactory<ITestTarget, TestTarget, TestFactory>();
        var target = fixture.Create<ITestTarget>();

        Assert.Multiple(
            () => Assert.NotNull(target),
            () => Assert.IsType<TestTarget>(target),
            () => Assert.IsType<ITestTarget>(target, exactMatch: false)
        );
    }

    [Fact]
    public void Can_construct_interface_request_type_using_factory_to_create_interface_type()
    {
        var fixture = new Fixture().WithTypeConstructedByFactory<ITestTarget, TestTarget, TestFactory2>();
        var target = fixture.Create<ITestTarget>();

        Assert.Multiple(
            () => Assert.NotNull(target),
            () => Assert.IsType<TestTarget>(target),
            () => Assert.IsType<ITestTarget>(target, exactMatch: false)
        );
    }
}

public class TestFactory
{
    private readonly string _name;
    private readonly int _value;

    public TestFactory(string name, int value)
    {
        _name=name;
        _value=value;
    }

    public TestTarget CreateTestTarget()
    {
        return new TestTarget(_name, _value);
    }
}

public class TestFactory2
{
    private readonly string _name;
    private readonly int _value;

    public TestFactory2(string name, int value)
    {
        _name=name;
        _value=value;
    }

    public ITestTarget CreateTestTarget()
    {
        return new TestTarget(_name, _value);
    }
}


public interface ITestTarget
{
}

public class TestTarget : ITestTarget
{
    internal TestTarget(string name, int value)
    {
        Name=name;
        Value=value;
    }

    public string Name { get; }
    public int Value { get; }
}
