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