namespace AutoFixture.Extensions.TestHelpers.Specification;

public class TestHelpersSpecification
{
    [Fact]
    public void Can_assert_on_private_constructor()
    {
        try
        {
            TestHelpers.AssertConstructorThrowsOnNullArgs<MyTest>();
        }
        catch (Exception)
        {
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_assert_on_factory_method()
    {
        try
        {
            TestHelpers.AssertMethodThrowsOnNullParameters<MyTest>(nameof(MyTest.Create));
        }
        catch (Exception)
        {
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_assert_on_private_method()
    {
        try
        {
            TestHelpers.AssertMethodThrowsOnNullParameters<MyTest>("Fix");
        }
        catch (Exception)
        {
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_assert_on_equality()
    {
        try
        {
            TestHelpers.AssertTypeImplementsEquality<MyTest2>();
        }
        catch (Exception)
        {
            return;            
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_assert_on_equality2()
    {
        TestHelpers.AssertTypeImplementsEquality<TestClass3>();
    }
}

public class MyTest
{
    private readonly string _input;

    private MyTest(string input)
    {
        _input = input;
    }

    public static MyTest Create(string input)
    {
        return new MyTest(input);
    }


    private string Fix(string input)
    {
        return input ?? _input;
    }
}

public class MyTest2
{
    private readonly int _id;
    private readonly string _note;

    public MyTest2(int id, string note)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(note);
        _id=id;
        _note=note;
    }

    public override bool Equals(object? obj)
    {
        if (obj is MyTest2 other) 
        {
            return other._note == _id.ToString();
        }

        return false;
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

}

public record TestClass3(string Input, int Input2);