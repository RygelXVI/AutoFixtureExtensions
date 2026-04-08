using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using Jouska.AutoFixture.Extensions.TestHelpers.Behaviors;
using System.Reflection;

namespace Jouska.AutoFixture.Extensions.TestHelpers;

public static class TestHelpers
{
    // TODO: add xml comments

    public static void AssertConstructorThrowsOnNullArgs<T>(params IBehaviorExpectation[] behaviourExpectations)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        AssertConstructorThrowsOnNullArgs<T>(fixture, behaviourExpectations);
    }

    public static void AssertConstructorThrowsOnNullArgs<T>(IFixture fixture, params IBehaviorExpectation[] behaviourExpectations)
    {
        var assertion = behaviourExpectations.Length == 0
            ? new GuardClauseAssertion(fixture)
            : new GuardClauseAssertion(fixture, new CompositeBehaviorExpectation(behaviourExpectations));

        assertion.Verify(typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    public static void AssertConstructorThrowsOnNullArgs<T>(CompositeBehaviorExpectation compositeBehaviorExpectation)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        var assertion = new GuardClauseAssertion(fixture, compositeBehaviorExpectation);
        assertion.Verify(typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    public static void AssertConstructorThrowsOnNullArgs<T>(IFixture fixture, CompositeBehaviorExpectation compositeBehaviorExpectation)
    {
        var assertion = new GuardClauseAssertion(fixture, compositeBehaviorExpectation);
        assertion.Verify(typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    public static void AssertMethodThrowsOnNullParameters<T>(string methodName)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        var assertion = new GuardClauseAssertion(fixture);
        assertion.Verify(typeof(T).GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
    }

    public static void AssertTypeImplementsBasicEquality<T>()
    {
        var fixture = new Fixture();
        var assertion = fixture.Create<BasicEqualityAssertion>();
        assertion.Verify(typeof(T));
    }

    public static void AssertTypeImplementEqualityComparer<T>()
    {
        var fixture = new Fixture();
        var assertion = fixture.Create<FullEqualityComparerAssertion>();
        assertion.Verify(typeof(T));
    }
}

