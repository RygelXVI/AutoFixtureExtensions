using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using Jouska.AutoFixture.Extensions.TestHelpers.Behaviors;
using System.Reflection;

namespace Jouska.AutoFixture.Extensions.TestHelpers;

public static class TestHelpers
{
    /// <summary>
    /// Asserts that the constructor of type T throws an ArgumentNullException when any of its parameters are null.<br/>
    /// All public and non-public constructors will be asserted on.<br/>
    /// Uses default behaviour expectations (null reference, empty guid)<br/>
    /// Invoked by:<br/><br/>
    /// <code>
    /// TestHelpers.AssertConstructorThrowsOnNullArgs&lt;MyClass&gt;();
    /// </code>
    /// </summary>
    /// <typeparam name="T">the type containing the constructor</typeparam>
    /// <param name="behaviourExpectations">the behavior expectations for the assertion</param>
    public static void AssertConstructorThrowsOnNullArgs<T>(params IBehaviorExpectation[] behaviourExpectations)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        AssertConstructorThrowsOnNullArgs<T>(fixture, behaviourExpectations);
    }

    /// <summary>
    /// Asserts that the constructor of type T throws an ArgumentNullException when any of its parameters are null.<br/>
    /// All public and non-public constructors will be asserted on.<br/>
    /// Uses default behaviour expectations (null reference, empty guid)<br/>
    /// Allows a fixture with customizations to be passed in for the assertion.<br/>
    /// Invoked by:<br/><br/>
    /// <code>
    /// var fixture = new Fixture().Customize(/* ... ellided ... */);
    /// TestHelpers.AssertConstructorThrowsOnNullArgs&lt;MyClass&gt;(fixture);
    /// </code>
    /// </summary>
    /// <typeparam name="T">the type containing the constructor</typeparam>
    /// <param name="fixture">the fixture to use for generation guard clause assertions</param>
    /// <param name="behaviourExpectations">the behavior expectations for the assertion</param>
    public static void AssertConstructorThrowsOnNullArgs<T>(IFixture fixture, params IBehaviorExpectation[] behaviourExpectations)
    {
        var assertion = behaviourExpectations.Length == 0
            ? new GuardClauseAssertion(fixture)
            : new GuardClauseAssertion(fixture, new CompositeBehaviorExpectation(behaviourExpectations));

        assertion.Verify(typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    /// <summary>
    /// Asserts that the constructor of type T throws an ArgumentNullException when any of its parameters are null.<br/>
    /// All public and non-public constructors will be asserted on.<br/>
    /// Uses default behaviour expectations (null reference, empty guid)<br/>
    /// Invoked by:<br/><br/>
    /// <code>
    /// var compositeBehaviorExpectation = new CompositeBehaviorExpectation(new NullReferenceBehaviorExpectation(), new EmptyStringBehaviorExpectation()));
    /// TestHelpers.AssertConstructorThrowsOnNullArgs&lt;MyClass&gt;(compositeBehaviorExpectation);
    /// </code>
    /// </summary>
    /// <typeparam name="T">the type containing the constructor</typeparam>
    /// <param name="compositeBehaviorExpectation">the composite behavior expectation for the assertion</param>
    public static void AssertConstructorThrowsOnNullArgs<T>(CompositeBehaviorExpectation compositeBehaviorExpectation)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        var assertion = new GuardClauseAssertion(fixture, compositeBehaviorExpectation);
        assertion.Verify(typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    /// <summary>
    /// Asserts that the constructor of type T throws an ArgumentNullException when any of its parameters are null.<br/>
    /// All public and non-public constructors will be asserted on.<br/>
    /// Uses default behaviour expectations (null reference, empty guid)<br/>
    /// Invoked by:<br/><br/>
    /// <code>
    /// var fixture = new Fixture().Customize(/* ... ellided ... */);
    /// var compositeBehaviorExpectation = new CompositeBehaviorExpectation(new NullReferenceBehaviorExpectation(), new EmptyStringBehaviorExpectation()));
    /// TestHelpers.AssertConstructorThrowsOnNullArgs&lt;MyClass&gt;(fixture, compositeBehaviorExpectation);
    /// </code>
    /// </summary>
    /// <typeparam name="T">the type containing the constructor</typeparam>
    /// <param name="fixture">the fixture to use for generation guard clause assertions</param>
    /// <param name="compositeBehaviorExpectation">the composite behavior expectation for the assertion</param>
    public static void AssertConstructorThrowsOnNullArgs<T>(IFixture fixture, CompositeBehaviorExpectation compositeBehaviorExpectation)
    {
        var assertion = new GuardClauseAssertion(fixture, compositeBehaviorExpectation);
        assertion.Verify(typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    /// <summary>
    /// Asserts that the specified method in type T throws an ArgumentNullException when any of its parameters are null.<br/>
    /// </summary>
    /// <typeparam name="T">the type containing the method</typeparam>
    /// <param name="methodName">the name of the method to assert on</param>
    public static void AssertMethodThrowsOnNullParameters<T>(string methodName)
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        var assertion = new GuardClauseAssertion(fixture);
        assertion.Verify(typeof(T).GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
    }

    /// <summary>
    /// Asserts that the specified type implements basic equality contracts.
    /// </summary>
    /// <typeparam name="T">the type to assert on</typeparam>
    public static void AssertTypeImplementsBasicEquality<T>()
    {
        var fixture = new Fixture();
        var assertion = fixture.Create<BasicEqualityAssertion>();
        assertion.Verify(typeof(T));
    }

    /// <summary>
    /// Asserts that the specified type implements IEqualityComparer.
    /// </summary>
    /// <typeparam name="T">the type to assert on</typeparam>
    public static void AssertTypeImplementEqualityComparer<T>()
    {
        var fixture = new Fixture();
        var assertion = fixture.Create<FullEqualityComparerAssertion>();
        assertion.Verify(typeof(T));
    }
}
