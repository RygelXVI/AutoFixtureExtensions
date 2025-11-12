using AutoFixture.Idioms;

namespace AutoFixture.Extensions.TestHelpers.Behaviors;

public class CompositeBehaviorExpectationBuilder
{
    private readonly List<IBehaviorExpectation> _behaviourExpectations;

    private CompositeBehaviorExpectationBuilder(List<IBehaviorExpectation> behaviorExpectations)
    {
        _behaviourExpectations = behaviorExpectations;
    }

    public static CompositeBehaviorExpectationBuilder WithNullReferenceExpectation(params string[] excludedParameterNames) =>
        new([new NullReferenceWithExclusionsBehaviorExpectation(excludedParameterNames)]);

    public static CompositeBehaviorExpectationBuilder WithNoBehaviors() =>
        new([]);

    public CompositeBehaviorExpectationBuilder WithBehaviorExpectation(IBehaviorExpectation behaviorExpectation)
    {
        _behaviourExpectations.Add(behaviorExpectation);
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithStringNullOrWhiteSpaceExpectation()
    {
        _behaviourExpectations.Add(new WhiteSpaceStringBehaviorExpectation());
        _behaviourExpectations.Add(new EmptyStringBehaviorExpectation());
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithEmptyGuidExpectation()
    {
        _behaviourExpectations.Add(new EmptyGuidBehaviorExpectation());
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithNonNegativeDoubleExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<double>(-1d));
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithNonNegativeIntegersExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<int>(-1));
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithNonNegativeLongExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<long>(-1L));
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithNonNegativeTimeSpanExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<TimeSpan>(TimeSpan.FromSeconds(-1)));
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithInvalidArgumentByNameExpectation<T>(
        T invalidValue,
        string parameterName)
    {
        _behaviourExpectations.Add(new InvalidArgumentByNameBehaviorExpectation<T>(invalidValue, parameterName));
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithInvalidArgumentByTypeExpectation<T>(T invalidValue)
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<T>(invalidValue));
        return this;
    }

    public CompositeBehaviorExpectationBuilder WithDefinedEnumExpectation<T>() where T : Enum
    {
        var value = (T)(object)int.MaxValue;
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<T>(value));
        return this;
    }

    public CompositeBehaviorExpectation Build() =>
        new(_behaviourExpectations);
}
