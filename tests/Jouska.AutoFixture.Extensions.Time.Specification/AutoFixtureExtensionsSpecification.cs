using AutoFixture;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Jouska.AutoFixture.Extensions.Time.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_register_time_provider()
    {
        var fixture = new Fixture().WithFakeTimeProvider();

        var subject = fixture.Freeze<TimeProviderTestSubject>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.NotNull(subject.TimeProvider),
            () => Assert.IsType<FakeTimeProvider>(subject.TimeProvider)
        );
    }

    [Fact]
    public void Can_register_time_provider_with_custom_now()
    {
        var now = new DateTimeOffset(2025, 10, 10, 11, 0, 0, TimeSpan.Zero);
        var fixture = new Fixture().WithFakeTimeProvider(now);
        var subject = fixture.Freeze<TimeProviderTestSubject>();
        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.NotNull(subject.TimeProvider),
            () => Assert.IsType<FakeTimeProvider>(subject.TimeProvider),
            () => Assert.Equal(now, subject.TimeProvider.GetUtcNow())
        );
    }

}


public class TimeProviderTestSubject
{
    public TimeProviderTestSubject(TimeProvider timeProvider)
    {
        TimeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public TimeProvider TimeProvider { get; }
}