using AutoFixture.Extensions.Options.Helpers;
using AutoFixture.Kernel;
using Microsoft.Extensions.Options;

namespace AutoFixture.Extensions.Options;

public static class AutoFixtureExtensions
{
    public static IFixture WithOptions<TOption>(this IFixture fixture, TOption option) where TOption : class
    {
        var options = Microsoft.Extensions.Options.Options.Create(option);
        fixture.Register(() => options);
        
        return fixture;
    }

    public static IFixture WithOptionsMonitor<TOption>(this IFixture fixture, TOption value) where TOption : class, new()
    {
        var optionsMonitor = new TestOptionsMonitor<TOption>(value);
        fixture.Register(() => optionsMonitor);       
        fixture.Customizations.Add(new TypeRelay(typeof(IOptionsMonitor<TOption>), typeof(TestOptionsMonitor<TOption>)));
        
        return fixture;
    }

    public static IFixture WithOptionsSnapshot<TOption>(this IFixture fixture, TOption value) where TOption : class, new()
    {
        var optionsSnapshot = new TestOptionsSnapshot<TOption>(value);
        fixture.Register(() => optionsSnapshot);
        fixture.Customizations.Add(new TypeRelay(typeof(IOptionsSnapshot<TOption>), typeof(TestOptionsSnapshot<TOption>)));
        
        return fixture;
    }
}
