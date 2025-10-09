using AutoFixture.Extensions.Options.Helpers;
using AutoFixture.Kernel;
using Microsoft.Extensions.Options;

namespace AutoFixture.Extensions.Options;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Allows registrations of a specific instance of an IOptions&lt;T&gt; interface, e.g. IOptions&lt;SystemUnderTestOptions&gt; <br/>
    /// </summary>
    /// <typeparam name="TOption">the option type</typeparam>
    /// <param name="value">the options value to be wrapped</param>
    /// <returns>IFixture</returns>
    public static IFixture WithOptions<TOption>(this IFixture fixture, TOption option) where TOption : class
    {
        var options = Microsoft.Extensions.Options.Options.Create(option);
        fixture.Register(() => options);
        
        return fixture;
    }

    /// <summary>
    /// Allows registrations of a specific instance of an IOptionsMonitor&lt;T&gt; interface.<br/>
    /// Uses a simple implementation of the IOptionsMonitor&lt;T&gt; interface, which allows updating of the options value directly
    /// </summary>
    /// <typeparam name="TOption">the option type</typeparam>
    /// <param name="value">the options value to be wrapped</param>
    /// <returns>IFixture</returns>
    public static IFixture WithOptionsMonitor<TOption>(this IFixture fixture, TOption value) where TOption : class, new()
    {
        var optionsMonitor = new TestOptionsMonitor<TOption>(value);
        fixture.Register(() => optionsMonitor);       
        fixture.Customizations.Add(new TypeRelay(typeof(IOptionsMonitor<TOption>), typeof(TestOptionsMonitor<TOption>)));
        
        return fixture;
    }

    /// <summary>
    /// Allows registrations of a specific instance of an IOptionsSnapshot&lt;T&gt; interface, e.g. IOptionsSnapshot&lt;SystemUnderTestOptions&gt; <br/>
    /// </summary>
    /// <typeparam name="TOption">the option type</typeparam>
    /// <param name="value">the options value to be wrapped</param>
    /// <returns>IFixture</returns>
    public static IFixture WithOptionsSnapshot<TOption>(this IFixture fixture, TOption value) where TOption : class, new()
    {
        var optionsSnapshot = new TestOptionsSnapshot<TOption>(value);
        fixture.Register(() => optionsSnapshot);
        fixture.Customizations.Add(new TypeRelay(typeof(IOptionsSnapshot<TOption>), typeof(TestOptionsSnapshot<TOption>)));
        
        return fixture;
    }
}
