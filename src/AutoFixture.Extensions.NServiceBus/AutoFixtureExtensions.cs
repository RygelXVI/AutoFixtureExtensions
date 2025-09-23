using AutoFixture.Kernel;
using NServiceBus.Testing;

namespace AutoFixture.Extensions.NServiceBus;

public static class AutoFixtureExtensions
{
    public static IFixture WithTestableMessageSession(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(IMessageSession), typeof(TestableMessageSession)));
        fixture.Register(() => new TestableMessageSession());
        fixture.WithTestableMessageHandlerContext();

        return fixture;
    }

    public static IFixture WithTestableEndpointInstance(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(IEndpointInstance), typeof(TestableEndpointInstance)));
        fixture.Register(() => new TestableEndpointInstance());        
        fixture.WithTestableMessageHandlerContext();

        return fixture;
    }

    public static IFixture WithTestableMessageHandlerContext(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(IMessageHandlerContext), typeof(TestableMessageHandlerContext)));
        fixture.Register(() => new TestableMessageHandlerContext());

        return fixture;
    }
}
