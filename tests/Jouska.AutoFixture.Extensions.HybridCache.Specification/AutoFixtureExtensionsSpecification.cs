using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Jouska.AutoFixture.Extensions.HybridCache.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_inject_distributed_cache()
    {
        var fixture = new Fixture().WithDistributedCache();

        var subject = fixture.Freeze<IDistributedCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryDistributedCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_distributed_cache_with_utc_now_set()
    {
        var utcNow = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var fixture = new Fixture().WithDistributedCache(utcNow);

        var subject = fixture.Freeze<IDistributedCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryDistributedCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_distributed_cache_with_time_provider()
    {
        var utcNow = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(utcNow);

        var fixture = new Fixture().WithDistributedCache(fakeTimeProvider);

        var subject = fixture.Freeze<IDistributedCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryDistributedCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_distributed_cache_with_options_configuration()
    {
        var options = new MemoryDistributedCacheOptions
        {
            SizeLimit = 100
        };
        var fixture = new Fixture().WithDistributedCache(options);

        var subject = fixture.Freeze<IDistributedCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryDistributedCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_distributed_cache_with_options_configuration_action()
    {
        var fixture = new Fixture().WithDistributedCache(options =>
        {
            options.SizeLimit = 100;
        });

        var subject = fixture.Freeze<IDistributedCache>();
        
        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryDistributedCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_memory_cache()
    {
        var fixture = new Fixture().WithMemoryCache();

        var subject = fixture.Freeze<IMemoryCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_memory_cache_with_utc_now_set()
    {
        var utcNow = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var fixture = new Fixture().WithMemoryCache(utcNow);

        var subject = fixture.Freeze<IMemoryCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_memory_cache_with_time_provider()
    {
        var utcNow = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var fakeTimeProvider = new FakeTimeProvider(utcNow);

        var fixture = new Fixture().WithMemoryCache(fakeTimeProvider);

        var subject = fixture.Freeze<IMemoryCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_memory_cache_with_options_configuration()
    {
        var options = new MemoryCacheOptions
        {
            SizeLimit = 100
        };

        var fixture = new Fixture().WithMemoryCache(options);

        var subject = fixture.Freeze<IMemoryCache>();

        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryCache>(subject)
        );
    }

    [Fact]
    public void Can_inject_memory_cache_with_options_configuration_action()
    {
        var fixture = new Fixture().WithMemoryCache(options =>
        {
            options.SizeLimit = 100;
        });

        var subject = fixture.Freeze<IMemoryCache>();
        
        Assert.Multiple(
            () => Assert.NotNull(subject),
            () => Assert.IsType<MemoryCache>(subject)
        );
    }
}
