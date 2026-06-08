using AutoFixture;
using AutoFixture.Kernel;
using NServiceBus.Testing;

namespace Jouska.AutoFixture.Extensions.NServiceBus;

public static class AutoFixtureExtensions
{
    extension(IFixture fixture)
    {
        /// <summary>
        /// Registers an NServiceBus TestableMessageSession with the fixture. <br/>
        ///  <br/>
        /// You can access the endpoint instance in your tests by calling:<br/><br/>
        /// <code>
        /// var testableMessageSession = fixture.Freeze&lt;IMessageSession&gt;();
        /// </code>
        /// </summary>
        /// <returns>IFixture</returns>
        public IFixture WithTestableMessageSession()
        {
            fixture.Customizations.Add(new TypeRelay(typeof(IMessageSession), typeof(TestableMessageSession)));
            fixture.Register(() => new TestableMessageSession());

            return fixture;
        }

        /// <summary>
        /// Registers a TestableMessageHandlerContext with the fixture. <br/>
        /// To be used when testing message handlers, if you prefer asking the fixture for an IMessageHandlerContext instead of directly creating it. <br/>
        /// You can access the testableMessageHandlerContext in your tests by:<br/><br/>
        /// <code>
        /// var testableMessageHandlerContext = fixture.Freeze&lt;IMessageHandlerContext&gt;();
        /// </code>
        /// </summary>
        /// <returns>IFixture</returns>
        public IFixture WithTestableMessageHandlerContext()
        {
            fixture.Customizations.Add(new TypeRelay(typeof(IMessageHandlerContext), typeof(TestableMessageHandlerContext)));
            fixture.Register(() => new TestableMessageHandlerContext());

            return fixture;
        }
    }
}
