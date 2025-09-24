using AutoFixture.Kernel;
using Microsoft.Extensions.Time.Testing;

namespace AutoFixture.Extensions.Time;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Registers a FakeTimeProvider with the fixture, allowing easy testing of code that uses System.TimeProvider.<br/>
    /// You can access this by:<br/><br/>
    /// <code>
    /// var fakeTimeProvider = fixture.Freeze&lt;TimeProvider&gt;();
    /// </code>
    /// </summary>
    /// <returns>IFixture</returns>
    public static IFixture WithFakeTimeProvider(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TimeProvider), typeof(FakeTimeProvider)));
        fixture.Register(() => new FakeTimeProvider());

        return fixture;
    }


    /// <summary>
    /// Registers a FakeTimeProvider with the fixture, allowing easy testing of code that uses System.TimeProvider. <br/>
    /// Allows you to set the current time of the TimeProvider at the moment of registration.<br/>
    /// You can access this by:<br/><br/>
    /// <code>
    /// fixture.WithFakeTimeProvider(new DateTimeOffset(2025, 10, 10, 11, 0, 0));
    /// var fakeTimeProvider = fixture.Freeze&lt;TimeProvider&gt;();
    /// </code>
    /// </summary>
    /// <param name="now">the starting time for the TimeProvider to use</param>
    /// <returns>IFixture</returns>
    public static IFixture WithFakeTimeProvider(this IFixture fixture, DateTimeOffset now)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TimeProvider), typeof(FakeTimeProvider)));
        fixture.Register(() => new FakeTimeProvider(now));

        return fixture;
    }
}
