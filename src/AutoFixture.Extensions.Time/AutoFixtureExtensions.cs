using AutoFixture.Kernel;
using Microsoft.Extensions.Time.Testing;

namespace AutoFixture.Extensions.Time;

public static class AutoFixtureExtensions
{
    public static IFixture WithFakeTimeProvider(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TimeProvider), typeof(FakeTimeProvider)));
        fixture.Register(() => new FakeTimeProvider());

        return fixture;
    }

    public static IFixture WithFakeTimeProvider(this IFixture fixture, DateTimeOffset now)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TimeProvider), typeof(FakeTimeProvider)));
        fixture.Register(() => new FakeTimeProvider(now));

        return fixture;
    }
}
