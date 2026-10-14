using AutoFixture;
using AutoFixture.Kernel;
using NodaTime;
using NodaTime.Testing;
using NodaTime.Testing.TimeZones;
using NodaTime.TimeZones;

namespace Jouska.AutoFixture.Extensions.NodaTime;

public static class AutoFixtureExtensions
{
    extension(IFixture fixture)
    {
        #region FakeClock

        /// <summary>
        /// Registers a FakeClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="configure">A function that configures the FakeClock</param>
        /// <returns>IFixture</returns>
        public IFixture WithClock(Func<Instant> configure) =>
            fixture.WithRegistration<IClock, FakeClock>(new FakeClock(configure.Invoke()));

        /// <summary>
        /// Registers a FakeClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="initialInstant">The initial instant for the FakeClock</param>
        /// <returns>IFixture</returns>
        public IFixture WithClock(Instant initialInstant) =>
            fixture.WithRegistration<IClock, FakeClock>(new FakeClock(initialInstant));

        /// <summary>
        /// Registers a FakeClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="dateTime">The initial date and time for the FakeClock</param>
        /// <exception cref="ArgumentException">Thrown when the provided DateTime is not in UTC.</exception>
        /// <returns>IFixture</returns>
        public IFixture WithClock(DateTime dateTime) =>
            WithClock(fixture, Instant.FromDateTimeUtc(dateTime));

        /// <summary>
        /// Registers a FakeClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="dateTimeOffset">The initial date and time for the FakeClock</param>
        /// <returns>IFixture</returns>
        public IFixture WithClock(DateTimeOffset dateTimeOffset) =>
            WithClock(fixture, Instant.FromDateTimeOffset(dateTimeOffset));

        /// <summary>
        /// Registers a FakeClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="initialInstant">initial date and time for the FakeClock</param>
        /// <param name="autoAdvance">duration to advance the clock on each read</param>
        /// <returns>IFixture</returns>
        public IFixture WithClock(Func<Instant> initialInstant, Func<Duration> autoAdvance) =>
            fixture.WithRegistration<IClock, FakeClock>(new FakeClock(initialInstant.Invoke(), autoAdvance.Invoke()));

        /// <summary>
        /// Registers a FakeClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="initialInstant">initial date and time for the FakeClock</param>
        /// <param name="autoAdvance">duration to advance the clock on each read</param>
        /// <returns>IFixture</returns>
        public IFixture WithClock(Instant initialInstant, Duration autoAdvance) =>
            fixture.WithRegistration<IClock, FakeClock>(new FakeClock(initialInstant, autoAdvance));

        /// <summary>
        /// Registers a FakeClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="initialInstant">initial date and time for the FakeClock</param>
        /// <param name="autoAdvance">duration to advance the clock on each read</param>
        /// <returns>IFixture</returns>
        public IFixture WithClock(Instant initialInstant, TimeSpan autoAdvance) =>
            WithClock(fixture, initialInstant, Duration.FromTimeSpan(autoAdvance));

        #endregion FakeClock

        #region ZonedClock

        /// <summary>
        /// Registers a ZonedClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="instant">instant for the ZonedClock</param>
        /// <param name="dateTimeZone">date time zone for the ZonedClock</param>
        /// <param name="calendarSystem">calendar system for the ZonedClock</param>
        /// <returns>IFixture</returns>
        public IFixture WithZonedClock(Instant instant, DateTimeZone dateTimeZone, CalendarSystem calendarSystem) =>
            fixture.WithRegistration<IClock, ZonedClock>(new ZonedClock(new FakeClock(instant), dateTimeZone, calendarSystem));

        /// <summary>
        /// Registers a ZonedClock instance with the fixture, allowing easy testing of code that uses IClock.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var clock = fixture.Freeze&lt;IClock&gt;();
        /// </code>
        /// </summary>
        /// <param name="instant">instant for the ZonedClock</param>
        /// <param name="dateTimeZone">date time zone for the ZonedClock</param>
        /// <param name="calendarSystem">calendar system for the ZonedClock</param>
        /// <returns>IFixture</returns>
        public IFixture WithZonedClock(Func<Instant> instant, Func<DateTimeZone> dateTimeZone, Func<CalendarSystem> calendarSystem) =>
            fixture.WithRegistration<IClock, ZonedClock>(new ZonedClock(new FakeClock(instant.Invoke()), dateTimeZone.Invoke(), calendarSystem.Invoke()));

        #endregion ZonedClock

        #region datetimezone source and provider

        /// <summary>
        /// Registers a DateTimeZoneProvider instance with the fixture, allowing easy testing of code that uses IDateTimeZoneProvider.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var provider = fixture.Freeze&lt;IDateTimeZoneProvider&gt;();
        /// </code>
        /// </summary>
        /// <param name="dateTimeZoneSource">The DateTimeZoneSource to register</param>
        /// <returns>IFixture</returns>
        public IFixture WithDateTimeZoneProvider(IDateTimeZoneSource dateTimeZoneSource) =>
            fixture.WithRegistration<IDateTimeZoneProvider, DateTimeZoneCache>(new DateTimeZoneCache(dateTimeZoneSource));

        /// <summary>
        /// Registers a DateTimeZoneProvider instance with the fixture, allowing easy testing of code that uses IDateTimeZoneProvider.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var source = fixture.Freeze&lt;IDateTimeZoneSource&gt;();
        /// </code>
        /// </summary>
        /// <param name="dateTimeZoneSource">The DateTimeZoneSource to register</param>
        /// <returns>IFixture</returns>
        public IFixture WithDateTimeZoneSource(FakeDateTimeZoneSource dateTimeZoneSource) =>
            fixture.WithRegistration<IDateTimeZoneSource, FakeDateTimeZoneSource>(dateTimeZoneSource);

        /// <summary>
        /// Registers a DateTimeZoneProvider instance with the fixture, allowing easy testing of code that uses IDateTimeZoneProvider.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var source = fixture.Freeze&lt;IDateTimeZoneSource&gt;();
        /// </code>
        /// </summary>
        /// <param name="config">The configuration function for creating the DateTimeZoneSource</param>
        /// <returns>IFixture</returns>
        public IFixture WithDateTimeZoneSource(Func<FakeDateTimeZoneSource.Builder, FakeDateTimeZoneSource> config) =>
            fixture.WithRegistration<IDateTimeZoneSource, FakeDateTimeZoneSource>(config.Invoke([]));

        #endregion datetimezone source and provider

        #region transition time zones

        /// <summary>
        /// Registers a SingleTransitionDateTimeZone instance with the fixture, allowing easy testing of code that uses DateTimeZone.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var zone = fixture.Freeze&lt;DateTimeZone&gt;();
        /// </code>
        /// </summary>
        /// <param name="singleTransitionDateTimeZone">a single transition DateTimeZone</param>
        /// <returns>IFixture</returns>
        public IFixture WithSingleTransitionDateTimeZone(SingleTransitionDateTimeZone singleTransitionDateTimeZone) =>
            fixture.WithRegistration<DateTimeZone, SingleTransitionDateTimeZone>(singleTransitionDateTimeZone);

        /// <summary>
        /// Registers a SingleTransitionDateTimeZone instance with the fixture, allowing easy testing of code that uses DateTimeZone.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var zone = fixture.Freeze&lt;DateTimeZone&gt;();
        /// </code>
        /// </summary>
        /// <param name="singleTransitionDateTimeZone">configuration for a single transition DateTimeZone</param>
        /// <returns>IFixture</returns>
        public IFixture WithSingleTransitionDateTimeZone(Func<SingleTransitionDateTimeZone> singleTransitionDateTimeZone) =>
            fixture.WithRegistration<DateTimeZone, SingleTransitionDateTimeZone>(singleTransitionDateTimeZone.Invoke());

        /// <summary>
        /// Registers a MultiTransitionDateTimeZone instance with the fixture, allowing easy testing of code that uses DateTimeZone.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var zone = fixture.Freeze&lt;DateTimeZone&gt;();
        /// </code>
        /// </summary>
        /// <param name="multiTransitionDateTimeZone">configuration for a multi transition DateTimeZone</param>
        /// <returns>IFixture</returns>
        public IFixture WithMultiTransitionDateTimeZone(MultiTransitionDateTimeZone multiTransitionDateTimeZone) =>
            fixture.WithRegistration<DateTimeZone, MultiTransitionDateTimeZone>(multiTransitionDateTimeZone);

        /// <summary>
        /// Registers a MultiTransitionDateTimeZone instance with the fixture, allowing easy testing of code that uses DateTimeZone.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var zone = fixture.Freeze&lt;DateTimeZone&gt;();
        /// </code>
        /// </summary>
        /// <param name="multiTransitionDateTimeZone">configuration for a multi transition DateTimeZone</param>
        /// <returns>IFixture</returns>
        public IFixture WithMultiTransitionDateTimeZone(Func<MultiTransitionDateTimeZone.Builder, MultiTransitionDateTimeZone> multiTransitionDateTimeZone) =>
            fixture.WithRegistration<DateTimeZone, MultiTransitionDateTimeZone>(multiTransitionDateTimeZone.Invoke([]));

        #endregion transition time zones

        private IFixture WithRegistration<TFrom, TTo>(TTo instance)
        {
            fixture.Customizations.Add(new TypeRelay(typeof(TFrom), typeof(TTo)));
            fixture.Register(() => instance);
            return fixture;
        }
    }
}
    