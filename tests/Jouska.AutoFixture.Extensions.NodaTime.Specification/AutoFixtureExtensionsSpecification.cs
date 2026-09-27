using AutoFixture;
using NodaTime;
using NodaTime.Testing;
using NodaTime.Testing.TimeZones;
using NodaTime.TimeZones;

namespace Jouska.AutoFixture.Extensions.NodaTime.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_register_fake_clock_with_instant()
    {
        var fixture = new Fixture()
            .WithClock(Instant.FromUtc(2020, 10, 10, 10, 10));

        var subject = fixture.Freeze<IClock>();

        AssertOnSubject<IClock, FakeClock>(subject);
    }

    [Fact]
    public void Can_register_fake_clock_with_instant_factory()
    {
        var fixture = new Fixture()
            .WithClock(() =>  Instant.FromUtc(2020, 10, 10, 10, 10));

        var subject = fixture.Create<IClock>();

        AssertOnSubject<IClock, FakeClock>(subject);
    }

    [Fact]
    public void Can_register_fake_clock_with_instant_from_utc_datetime()
    {
        var fixture = new Fixture()
            .WithClock(new DateTime(2022,10,10, 10,10,10, DateTimeKind.Utc));

        var subject = fixture.Create<IClock>();

        AssertOnSubject<IClock, FakeClock>(subject);
    }

    [Fact]
    public void Can_register_fake_clock_with_instant_from_datetime_offset()
    {
        var fixture = new Fixture()
            .WithClock(new DateTimeOffset(new DateTime(2022, 10, 10, 10, 10, 10, DateTimeKind.Utc)));

        var subject = fixture.Create<IClock>();

        AssertOnSubject<IClock, FakeClock>(subject);
    }

    [Fact]
    public void Can_register_fake_clock_with_instant_and_advance_duration()
    {
        var fixture = new Fixture()
            .WithClock(Instant.FromUtc(2020, 10, 10, 10, 10), Duration.FromDays(1));

        var subject = fixture.Freeze<IClock>();

        AssertOnSubject<IClock, FakeClock>(subject);
    }

    [Fact]
    public void Can_register_fake_clock_with_instant_and_advance_duration_factories()
    {
        var fixture = new Fixture()
            .WithClock(() => Instant.FromUtc(2020, 10, 10, 10, 10), () => Duration.FromDays(1));

        var subject = fixture.Freeze<IClock>();

        AssertOnSubject<IClock, FakeClock>(subject);
    }

    [Fact]
    public void Can_register_fake_clock_with_instant_and_advance_duration_timespan()
    {
        var fixture = new Fixture()
            .WithClock(Instant.FromUtc(2020, 10, 10, 10, 10), TimeSpan.FromDays(1));

        var subject = fixture.Freeze<IClock>();

        AssertOnSubject<IClock, FakeClock>(subject);
    }

    [Fact]
    public void Can_register_zoned_clock()
    {
        var fixture = new Fixture()
            .WithZonedClock(
                Instant.FromUtc(2020, 10, 10, 10, 10), 
                DateTimeZone.ForOffset(Offset.Zero), 
                CalendarSystem.Gregorian);

        var subject = fixture.Create<IClock>();

        AssertOnSubject<IClock, ZonedClock>(subject);
    }

    [Fact]
    public void Can_register_zoned_clock_with_factories()
    {
        var fixture = new Fixture()
            .WithZonedClock(
                () => Instant.FromUtc(2020, 10, 10, 10, 10),
                () => DateTimeZone.ForOffset(Offset.Zero),
                () => CalendarSystem.Gregorian);

        var subject = fixture.Create<IClock>();

        AssertOnSubject<IClock, ZonedClock>(subject);
    }

    [Fact]
    public void Can_register_datetime_zone_provider()
    {
        var fixture = new Fixture()
            .WithDateTimeZoneProvider(new FakeDateTimeZoneSource.Builder().Build());

        var subject = fixture.Create<IDateTimeZoneProvider>();

        AssertOnSubject<IDateTimeZoneProvider, DateTimeZoneCache>(subject);
    }

    [Fact]
    public void Can_register_datetime_zone_source()
    {
        var fixture = new Fixture()
            .WithDateTimeZoneSource(new FakeDateTimeZoneSource.Builder().Build());

        var subject = fixture.Create<IDateTimeZoneSource>();

        AssertOnSubject<IDateTimeZoneSource, FakeDateTimeZoneSource>(subject);
    }

    [Fact]
    public void Can_register_datetime_zone_source_with_factory()
    {
        var fixture = new Fixture()
            .WithDateTimeZoneSource(x => x.Build());

        var subject = fixture.Create<IDateTimeZoneSource>();

        AssertOnSubject<IDateTimeZoneSource, FakeDateTimeZoneSource>(subject);
    }

    [Fact]
    public void Can_register_single_transition_datetime_zone()
    {
        var fixture = new Fixture()
            .WithSingleTransitionDateTimeZone(new SingleTransitionDateTimeZone(Instant.FromUtc(2020,10,10,10,10), 1, 2));

        var subject = fixture.Create<DateTimeZone>();

        AssertOnSubject<DateTimeZone, SingleTransitionDateTimeZone>(subject);
    }

    [Fact]
    public void Can_register_single_transition_datetime_zone_with_factory()
    {
        var fixture = new Fixture()
            .WithSingleTransitionDateTimeZone(() => new SingleTransitionDateTimeZone(Instant.FromUtc(2020, 10, 10, 10, 10), 1, 2));

        var subject = fixture.Create<DateTimeZone>();

        AssertOnSubject<DateTimeZone, SingleTransitionDateTimeZone>(subject);
    }

    [Fact]
    public void Can_register_multiple_transition_datetime_zone()
    {
        var fixture = new Fixture()
            .WithMultiTransitionDateTimeZone(new MultiTransitionDateTimeZone.Builder().Build());

        var subject = fixture.Create<DateTimeZone>();

        AssertOnSubject<DateTimeZone, MultiTransitionDateTimeZone>(subject);
    }

    [Fact]
    public void Can_register_multiple_transition_datetime_zone_with_factory()
    {
        var fixture = new Fixture()
            .WithMultiTransitionDateTimeZone(x => x.Build());

        var subject = fixture.Create<DateTimeZone>();

        AssertOnSubject<DateTimeZone, MultiTransitionDateTimeZone>(subject);
    }

    private static void AssertOnSubject<TInterface, TImplementation>(TInterface subject)
    {
        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<TImplementation>(subject),
            () => Assert.IsType<TInterface>(subject, exactMatch: false)
        );
    }
}
