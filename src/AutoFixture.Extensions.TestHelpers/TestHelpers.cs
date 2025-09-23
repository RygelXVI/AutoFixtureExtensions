using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;

namespace AutoFixture.Extensions.TestHelpers;

public static class TestHelpers
{
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

        assertion.Verify(typeof(T).GetConstructors());
    }

    public static void AssertConstructorThrowsOnNullArgs<T>(CompositeBehaviorExpectation compositeBehaviorExpectation)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        var assertion = new GuardClauseAssertion(fixture, compositeBehaviorExpectation);
        assertion.Verify(typeof(T).GetConstructors());
    }

    public static void AssertConstructorThrowsOnNullArgs<T>(IFixture fixture, CompositeBehaviorExpectation compositeBehaviorExpectation)
    {
        var assertion = new GuardClauseAssertion(fixture, compositeBehaviorExpectation);
        assertion.Verify(typeof(T).GetConstructors());
    }
}
