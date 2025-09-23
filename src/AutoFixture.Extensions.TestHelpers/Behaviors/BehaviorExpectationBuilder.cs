using AutoFixture.Idioms;

namespace AutoFixture.Extensions.TestHelpers.Behaviors;

public class BehaviorExpectationBuilder
{
    private readonly List<IBehaviorExpectation> _behaviourExpectations;

    private BehaviorExpectationBuilder(List<IBehaviorExpectation> behaviorExpectations)
    {
        _behaviourExpectations = behaviorExpectations;
    }

    public static BehaviorExpectationBuilder WithNullReferenceExpectation(params string[] excludedParameterNames) =>
        new([new NullReferenceBehaviorExpectation(excludedParameterNames)]);

    public static BehaviorExpectationBuilder WithNoBehaviors() =>
        new([]);

    public BehaviorExpectationBuilder WithBehaviorExpectation(IBehaviorExpectation behaviorExpectation)
    {
        _behaviourExpectations.Add(behaviorExpectation);
        return this;
    }

    public BehaviorExpectationBuilder WithStringNullOrWhiteSpaceExpectation()
    {
        _behaviourExpectations.Add(new WhiteSpaceStringBehaviorExpectation());
        _behaviourExpectations.Add(new EmptyStringBehaviorExpectation());
        return this;
    }

    public BehaviorExpectationBuilder WithEmptyGuidExpectation()
    {
        _behaviourExpectations.Add(new EmptyGuidBehaviorExpectation());
        return this;
    }

    public BehaviorExpectationBuilder WithNonNegativeDoubleExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<double>(-1d));
        return this;
    }

    public BehaviorExpectationBuilder WithNonNegativeIntegersExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<int>(-1));
        return this;
    }

    public BehaviorExpectationBuilder WithNonNegativeLongExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<long>(-1L));
        return this;
    }

    public BehaviorExpectationBuilder WithNonNegativeTimeSpanExpectation()
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<TimeSpan>(TimeSpan.FromSeconds(-1)));
        return this;
    }

    public BehaviorExpectationBuilder WithInvalidArgumentByNameExpectation<T>(
        T invalidValue,
        string parameterName)
    {
        _behaviourExpectations.Add(new InvalidArgumentByNameBehaviorExpectation<T>(invalidValue, parameterName));
        return this;
    }

    public BehaviorExpectationBuilder WithInvalidArgumentByTypeExpectation<T>(T invalidValue)
    {
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<T>(invalidValue));
        return this;
    }

    public BehaviorExpectationBuilder WithDefinedEnumExpectation<T>() where T : Enum
    {
        var value = (T)(object)int.MaxValue;
        _behaviourExpectations.Add(new InvalidArgumentByTypeBehaviorExpectation<T>(value));
        return this;
    }

    public CompositeBehaviorExpectation Build() =>
        new(_behaviourExpectations);
}
