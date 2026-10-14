using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.Extensions.Time.Testing;

namespace Jouska.AutoFixture.Extensions.Time;

public static class AutoFixtureExtensions
{
    extension(IFixture fixture)
    {
        /// <summary>
        /// Registers a singleton FakeTimeProvider instance with the fixture, allowing easy testing of code that uses System.TimeProvider.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var fakeTimeProvider = fixture.Freeze&lt;TimeProvider&gt;();
        /// </code>
        /// </summary>
        /// <returns>IFixture</returns>
        public IFixture WithFakeTimeProvider() => 
            fixture.Inject<TimeProvider, FakeTimeProvider>(new FakeTimeProvider());

        /// <summary>
        /// Registers a singleton FakeTimeProvider instance with the fixture, allowing easy testing of code that uses System.TimeProvider. <br/>
        /// Allows you to set the current time of the TimeProvider at the moment of registration.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// fixture.WithFakeTimeProvider(new DateTimeOffset(2025, 10, 10, 11, 0, 0));
        /// var fakeTimeProvider = fixture.Freeze&lt;TimeProvider&gt;();
        /// </code>
        /// </summary>
        /// <param name="now">the starting time for the TimeProvider to use</param>
        /// <returns>IFixture</returns>
        public IFixture WithFakeTimeProvider(DateTimeOffset now) =>
            fixture.Inject<TimeProvider, FakeTimeProvider>(new FakeTimeProvider(now));

        private IFixture Inject<TFrom, TTo>(TTo instance)
        {
            fixture.Customizations.Add(new TypeRelay(typeof(TFrom), typeof(TTo)));
            fixture.Inject(instance);
            return fixture;
        }
    }
}
