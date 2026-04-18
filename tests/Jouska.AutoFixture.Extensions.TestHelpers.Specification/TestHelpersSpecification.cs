using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using Jouska.AutoFixture.Extensions.TestHelpers.Behaviors;
using System.Diagnostics.CodeAnalysis;

namespace Jouska.AutoFixture.Extensions.TestHelpers.Specification;

public class TestHelpersSpecification
{
    [Fact]
    public void Can_assert_on_private_constructor()
    {
        try
        {
            TestHelpers.AssertConstructorThrowsOnNullArgs<MyTest>();
        }
        catch (GuardClauseException)
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
        catch (GuardClauseException)
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
        catch (GuardClauseException)
        {
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_add_expectation_to_constructor_assertion_checks()
    {
        var whitespaceExpectation = new WhiteSpaceStringBehaviorExpectation();

        try
        {
            TestHelpers.AssertConstructorThrowsOnNullArgs<MyTest>(whitespaceExpectation);
        }
        catch (GuardClauseException)
        {
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_add_composite_expectations_to_constructor_assertion_checks()
    {
        var expectations = CompositeBehaviorExpectationBuilder
            .WithNoBehaviors()
            .WithStringNullOrWhiteSpaceExpectation()
            .Build();

        try
        {
            TestHelpers.AssertConstructorThrowsOnNullArgs<MyTest>(expectations);
        }
        catch (GuardClauseException)
        {
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_add_fixture_and_composite_expectations_to_constructor_assertion_checks()
    {
        var expectations = CompositeBehaviorExpectationBuilder
            .WithNullReferenceExpectation()
            .WithStringNullOrWhiteSpaceExpectation()
            .Build();

        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        try
        {
            TestHelpers.AssertConstructorThrowsOnNullArgs<MyTest>(fixture, expectations);
        }
        catch (GuardClauseException)
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
            TestHelpers.AssertTypeImplementsBasicEquality<MyTest2>();
        }
        catch (EqualsOverrideException ex)
        {
            Assert.NotNull(ex);
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void Can_assert_on_equality2()
    {
        TestHelpers.AssertTypeImplementsBasicEquality<TestClass3>();
    }

    [Fact]
    public void Can_assert_on_equality_comparer()
    {
        try
        {
            TestHelpers.AssertTypeImplementEqualityComparer<MyTest4>();
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


public class MyTest4 : IEqualityComparer<MyTest4>
{
    private readonly int _id;
    private readonly string _note;

    public MyTest4(int id, string note)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(note);
        _id=id;
        _note=note;
    }

    public override bool Equals(object? obj)
    {
        if (obj is MyTest2 other)
        {
            return other.ToString() == _id.ToString();
        }

        return false;
    }

    public bool Equals(MyTest4? x, MyTest4? y)
    {
        return x?.Equals(y) ?? false;
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    public int GetHashCode([DisallowNull] MyTest4 obj)
    {
        return HashCode.Combine(obj._id.GetHashCode(), obj._note.GetHashCode(), obj._id.GetHashCode());
    }
}

public record TestClass3(string Input, int Input2);