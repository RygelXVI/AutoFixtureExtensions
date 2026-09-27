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

        public IFixture WithClock(Func<Instant> configure)
        {
            fixture.Register(() => new FakeClock(configure.Invoke()));
            return fixture.WithTypeRelay<IClock, FakeClock>();
        }

        public IFixture WithClock(Instant initialInstant)
        {
            fixture.Register(() => new FakeClock(initialInstant));
            return fixture.WithTypeRelay<IClock, FakeClock>();
        }

        public IFixture WithClock(DateTime dateTime) =>
            WithClock(fixture, Instant.FromDateTimeUtc(dateTime));

        public IFixture WithClock(DateTimeOffset dateTimeOffset) =>
            WithClock(fixture, Instant.FromDateTimeOffset(dateTimeOffset));

        public IFixture WithClock(Func<Instant> initialInstant, Func<Duration> autoAdvance)
        {
            fixture.Register(() => new FakeClock(initialInstant.Invoke(), autoAdvance.Invoke()));
            return fixture.WithTypeRelay<IClock, FakeClock>();
        }

        public IFixture WithClock(Instant initialInstant, Duration autoAdvance)
        {
            fixture.Register(() => new FakeClock(initialInstant, autoAdvance));
            return fixture.WithTypeRelay<IClock, FakeClock>();
        }

        public IFixture WithClock(Instant initialInstant, TimeSpan autoAdvance) =>
            WithClock(fixture, initialInstant, Duration.FromTimeSpan(autoAdvance));

        #endregion FakeClock

        #region ZonedClock

        public IFixture WithZonedClock(Instant instant, DateTimeZone dateTimeZone, CalendarSystem calendarSystem)
        {
            fixture.Register(() => new ZonedClock(new FakeClock(instant), dateTimeZone, calendarSystem));
            return fixture.WithTypeRelay<IClock, ZonedClock>();
        }

        public IFixture WithZonedClock(Func<Instant> instant, Func<DateTimeZone> dateTimeZone, Func<CalendarSystem> calendarSystem)
        {
            fixture.Register(() => new ZonedClock(new FakeClock(instant.Invoke()), dateTimeZone.Invoke(), calendarSystem.Invoke()));
            return fixture.WithTypeRelay<IClock, ZonedClock>();
        }

        #endregion ZonedClock

        #region datetimezone source and provider

        public IFixture WithDateTimeZoneProvider(IDateTimeZoneSource dateTimeZoneSource)
        {
            fixture.Register(() => new DateTimeZoneCache(dateTimeZoneSource));
            return fixture.WithTypeRelay<IDateTimeZoneProvider, DateTimeZoneCache>();
        }

        public IFixture WithDateTimeZoneSource(IDateTimeZoneSource dateTimeZoneSource)
        {
            fixture.Register(() => dateTimeZoneSource);
            return fixture.WithTypeRelay<IDateTimeZoneSource, FakeDateTimeZoneSource>();
        }

        public IFixture WithDateTimeZoneSource(Func<FakeDateTimeZoneSource.Builder, FakeDateTimeZoneSource> config)
        {
            fixture.Register(() => config.Invoke([]));
            return fixture.WithTypeRelay<IDateTimeZoneSource, FakeDateTimeZoneSource>();
        }

        #endregion datetimezone source and provider

        #region transition time zones

        public IFixture WithSingleTransitionDateTimeZone(SingleTransitionDateTimeZone singleTransitionDateTimeZone)
        {
            fixture.Register(() => singleTransitionDateTimeZone);
            return fixture.WithTypeRelay<DateTimeZone, SingleTransitionDateTimeZone>();
        }

        public IFixture WithSingleTransitionDateTimeZone(Func<SingleTransitionDateTimeZone> singleTransitionDateTimeZone)
        {
            fixture.Register(() => singleTransitionDateTimeZone.Invoke());
            return fixture.WithTypeRelay<DateTimeZone, SingleTransitionDateTimeZone>();
        }

        public IFixture WithMultiTransitionDateTimeZone(MultiTransitionDateTimeZone multiTransitionDateTimeZone)
        {
            fixture.Register(() => multiTransitionDateTimeZone);
            return fixture.WithTypeRelay<DateTimeZone, MultiTransitionDateTimeZone>();
        }

        public IFixture WithMultiTransitionDateTimeZone(Func<MultiTransitionDateTimeZone.Builder, MultiTransitionDateTimeZone> multiTransitionDateTimeZone)
        {
            fixture.Register(() => multiTransitionDateTimeZone.Invoke([]));
            return fixture.WithTypeRelay<DateTimeZone, MultiTransitionDateTimeZone>();
        }

        #endregion transition time zones

        private IFixture WithTypeRelay<TFrom, TTo>()
        {
            fixture.Customizations.Add(new TypeRelay(typeof(TFrom), typeof(TTo)));
            return fixture;
        }
    }
}
    