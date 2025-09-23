# AutoFixture Extensions #

This repository contains a number of small helper libraries to make working with 
AutoFixture simpler.  The aim is to ease the configuration of AutoFixture, especially
when using it as a DI container.

## AutoFixture Extensions Common ##

Contains basic extension methods to help register interfaces, implementations, and named parameters.

## AutoFixture Extensions Http ##

Contains extension methods to help register HttpClient and HttpClientFactory implementations that make
use of the RichardSzalay.MockHttp mocking library internally.

## AutoFixture Extensions Logging ##

Contains an extension method to help register the FakeLogger<T> provided as part of the Microsoft.Extensions.Logging
framework.

## AutoFixture Extensions NServiceBus ##

Contains extension methods to help register testable instances of common NServiceBus abstractions used in 
application code (IMessageSession, IEndpointInstance, IMessageHandlerContext).

## AutoFixture Extensions Options ##

Contains extension methods to help create and register classes common used as part of the Microsoft.Extensions.Options
framework (IOptions<T>, IOptionsSnapshot<T>, IOptionsMonitor<T>).

Also contains simple implementations of IOptionsSnapshot<T> and IOptionsMonitor<T>.

## AutoFixture Extensions Serilog ##

Contains extension methods to help register Serilog logging, both directly and as a provider for the 
Microsoft.Extensions.Logging logger.

## AutoFixture Extensions TestHelpers ##

Contains extension methods, snippets, and AutoFixture behaviors to help write unit tests for guard clauses.

## AutoFixture Extensions Time ##

Contains extension methods for registering a TimeProvider from the Microsoft.Extensions.TimeProvider framework.

## AutoFixture Extensions Vogen ##

Contains extension methods to register custom builders to allow Vogen types to be constructed by the
fixture.