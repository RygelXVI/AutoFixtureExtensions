using AutoFixture;
using AutoFixture.Kernel;
using Jouska.AutoFixture.Extensions.HybridCache.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;

namespace Jouska.AutoFixture.Extensions.HybridCache;

public static class AutoFixtureExtensions
{
    extension(IFixture fixture)
    {
        /// <summary>
        /// Registers a singleton MemoryDistributedCache instance with the fixture, allowing easy testing of code that uses IDistributedCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var hybridCache = fixture.Freeze&lt;IDistributedCache&gt;();
        /// </code>
        /// </summary>
        /// <returns>IFixture</returns>
        public IFixture WithDistributedCache() => 
            fixture.Inject<IDistributedCache, MemoryDistributedCache>(new MemoryDistributedCache(CreateOptions<MemoryDistributedCacheOptions>()));

        /// <summary>
        /// Registers a singleton MemoryDistributedCache instance with the fixture with the cache's clock set to the specified time, 
        /// allowing easy testing of code that uses IDistributedCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var hybridCache = fixture.Freeze&lt;IDistributedCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="utcNow">The current time for the cache's clock</param>
        /// <returns>IFixture</returns>
        public IFixture WithDistributedCache(DateTimeOffset utcNow) => 
            fixture.Inject<IDistributedCache, MemoryDistributedCache>(new MemoryDistributedCache(CreateOptions<MemoryDistributedCacheOptions>(utcNow)));

        /// <summary>
        /// Registers a singleton MemoryDistributedCache instance with the fixture with the cache's clock using the specified time provider, 
        /// allowing easy testing of code that uses IDistributedCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var hybridCache = fixture.Freeze&lt;IDistributedCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="timeProvider">The time provider for the MemoryDistributedCache</param>
        /// <returns>IFixture</returns>
        public IFixture WithDistributedCache(TimeProvider timeProvider) => 
            fixture.Inject<IDistributedCache, MemoryDistributedCache>(new MemoryDistributedCache(CreateOptions<MemoryDistributedCacheOptions>(timeProvider)));

        /// <summary>
        /// Registers a singleton MemoryDistributedCache instance with the fixture with the specified options, allowing easy testing of code that uses IDistributedCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var hybridCache = fixture.Freeze&lt;IDistributedCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="memoryDistributedCacheOptions">The options for the MemoryDistributedCache</param>
        /// <returns>IFixture</returns>
        public IFixture WithDistributedCache(MemoryDistributedCacheOptions memoryDistributedCacheOptions)
        {
            var options = Options.Create(memoryDistributedCacheOptions);
            return fixture.Inject<IDistributedCache, MemoryDistributedCache>(new MemoryDistributedCache(options));
        }

        /// <summary>
        /// Registers a singleton MemoryDistributedCache instance with the fixture with the specified option configuration, allowing easy testing of code that uses IDistributedCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var hybridCache = fixture.Freeze&lt;IDistributedCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="configure">configuration action for the MemoryDistributedCacheOptions</param>
        /// <returns>IFixture</returns>
        public IFixture WithDistributedCache(Action<MemoryDistributedCacheOptions> configure) => 
            fixture.Inject<IDistributedCache, MemoryDistributedCache>(new MemoryDistributedCache(CreateOptions(configure)));

        /// <summary>
        /// Registers a singleton MemoryCache instance with the fixture, allowing easy testing of code that uses IMemoryCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var cache = fixture.Freeze&lt;IMemoryCache&gt;();
        /// </code>
        /// </summary>
        /// <returns>IFixture</returns>
        public IFixture WithMemoryCache() => 
            fixture.Inject<IMemoryCache, MemoryCache>(new MemoryCache(new MemoryCacheOptions()));

        /// <summary>
        /// Registers a singleton MemoryCache instance with the fixture with the cache's clock set to the specified time, allowing easy testing of code that uses IMemoryCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var cache = fixture.Freeze&lt;IMemoryCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="utcNow">The current time for the cache's clock</param>
        /// <returns>IFixture</returns>
        public IFixture WithMemoryCache(DateTimeOffset utcNow) =>
            fixture.Inject<IMemoryCache, MemoryCache>(new MemoryCache(CreateOptions<MemoryCacheOptions>(utcNow)));

        /// <summary>
        /// Registers a singleton MemoryCache instance with the fixture with the cache's clock using the specified time provider, allowing easy testing of code that uses IMemoryCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var cache = fixture.Freeze&lt;IMemoryCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="timeProvider">The time provider for the MemoryCache</param>
        /// <returns>IFixture</returns>
        public IFixture WithMemoryCache(TimeProvider timeProvider) =>
            fixture.Inject<IMemoryCache, MemoryCache>(new MemoryCache(CreateOptions<MemoryCacheOptions>(timeProvider)));

        /// <summary>
        /// Registers a singleton MemoryCache instance with the fixture with the specified options, allowing easy testing of code that uses IMemoryCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var cache = fixture.Freeze&lt;IMemoryCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="memoryCacheOptions">The options for the MemoryCache</param>
        /// <returns>IFixture</returns>
        public IFixture WithMemoryCache(MemoryCacheOptions memoryCacheOptions) =>
            fixture.Inject<IMemoryCache, MemoryCache>(new MemoryCache(memoryCacheOptions));

        /// <summary>
        /// Registers a singleton MemoryCache instance with the fixture with the specified option configuration, allowing easy testing of code that uses IMemoryCache.<br/>
        /// You can access this by:<br/><br/>
        /// <code>
        /// var cache = fixture.Freeze&lt;IMemoryCache&gt;();
        /// </code>
        /// </summary>
        /// <param name="configure">configuration action for the MemoryCacheOptions</param>
        /// <returns>IFixture</returns>
        /// 
        public IFixture WithMemoryCache(Action<MemoryCacheOptions> configure) =>
            fixture.Inject<IMemoryCache, MemoryCache>(new MemoryCache(CreateOptions(configure)));

        private IFixture Inject<TFrom, TTo>(TTo instance)
        {
            fixture.Customizations.Add(new TypeRelay(typeof(TFrom), typeof(TTo)));
            fixture.Inject(instance);
            return fixture;
        }
    }

    private static IOptions<T> CreateOptions<T>(DateTimeOffset utcNow) where T : MemoryCacheOptions, new() =>
        CreateOptions<T>(new TestClock(new FakeTimeProvider(utcNow)));

    private static IOptions<T> CreateOptions<T>(TimeProvider timeProvider) where T : MemoryCacheOptions, new() =>
        CreateOptions<T>(new TestClock(timeProvider));

    private static IOptions<T> CreateOptions<T>(ISystemClock systemClock) where T : MemoryCacheOptions, new()
    {
        var opt = new T
        {
            Clock = systemClock
        };
        return Options.Create<T>(opt);
    }

    private static IOptions<T> CreateOptions<T>() where T : MemoryCacheOptions, new()
    {
        var opt = new T();
        return Options.Create<T>(opt);
    }

    private static IOptions<T> CreateOptions<T>(Action<T> configure) where T : MemoryCacheOptions, new()
    {
        var opt = new T();
        configure(opt);
        return Options.Create<T>(opt);
    }
}
