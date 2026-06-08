using AutoFixture;
using NServiceBus.Testing;
using Xunit;

namespace Jouska.AutoFixture.Extensions.NServiceBus.Specification;

public class AutoFixtureExtensionsSpecification
{
    [Fact]
    public void Can_inject_testable_message_session()
    {
        var fixture = new Fixture().WithTestableMessageSession();
        var testSubject = fixture.Freeze<TestSubject>();

        var session = testSubject.MessageSession as TestableMessageSession;

        Assert.Multiple(
            () => Assert.IsType<TestSubject>(testSubject),
            () => Assert.IsType<IMessageSession>(testSubject.MessageSession, exactMatch: false),
            () => Assert.NotNull(session)
        );
    }

    [Fact]
    public void Can_inject_testable_message_handler_context()
    {
        var fixture = new Fixture().WithTestableMessageHandlerContext();

        var testSubject = fixture.Freeze<IMessageHandlerContext>();

        Assert.Multiple(
            () => Assert.IsType<TestableMessageHandlerContext>(testSubject),
            () => Assert.IsType<IMessageHandlerContext>(testSubject, exactMatch: false)
        );
    }
}

public class TestSubject
{
    public TestSubject(IMessageSession messageSession)
    {
        MessageSession = messageSession;
    }
    public IMessageSession MessageSession { get; }
}
