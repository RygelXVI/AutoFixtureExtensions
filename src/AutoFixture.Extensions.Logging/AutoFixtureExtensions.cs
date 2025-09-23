using AutoFixture.Kernel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace AutoFixture.Extensions.Logging;

public static class AutoFixtureExtensions
{
    public static IFixture WithMicrosoftLogger<T>(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(ILogger<T>), typeof(FakeLogger<T>)));
        fixture.Register(() => new FakeLogger<T>());

        return fixture;
    }

}
