# Jouska AutoFixture Extensions #

Jouska - a hypothetical conversation that you compulsively play out in your head

This repository contains a number of small helper libraries to make working with 
AutoFixture simpler.  The aim is to ease the configuration of AutoFixture, especially
when using it as a DI container.

## Jouska AutoFixture Extensions Common ##

Contains basic extension methods to help register interfaces, implementations, and named parameters.

## Jouska AutoFixture Extensions Szalay Http ##

Contains extension methods to help register `HttpClient` and `HttpClientFactory`  implementations that make
use of the `RichardSzalay.MockHttp` mocking library internally.

## Jouska AutoFixture Extensions Http Mockly ##

Contains extension methods to help register `HttpClient` and `HttpClientFactory`  implementations that make
use of the `Mockly` mocking library internally.

## Jouska AutoFixture Extensions Http ##

Contains extension methods to help register `HttpClient` and `HttpClientFactory`  implementations that make
use of a simple HttpDelegatingHandler that supports testing.

## Jouska AutoFixture Extensions Logging ##

Contains extension methods to help register the `FakeLogger<T>` provided as part of the `Microsoft.Extensions.Logging`
framework.

## Jouska AutoFixture Extensions NServiceBus ##

Contains extension methods to help register testable instances of common NServiceBus abstractions used in 
application code (`IMessageSession`, `IEndpointInstance`, `IMessageHandlerContext`).

## Jouska AutoFixture Extensions Options ##

Contains extension methods to help create and register classes common used as part of the `Microsoft.Extensions.Options`
framework (`IOptions<T>`, `IOptionsSnapshot<T>`, `IOptionsMonitor<T>`).

Also contains simple implementations of `IOptionsSnapshot<T>` and `IOptionsMonitor<T>` for use with testing.

## Jouska AutoFixture Extensions Serilog ##

Contains extension methods to help register `Serilog` logging, both directly and as a provider for the 
`Microsoft.Extensions.Logging` logger.

## Jouska AutoFixture Extensions TestHelpers ##

Contains extension methods, snippets, and AutoFixture behaviors to help write unit tests for guard clauses.

## Jouska AutoFixture Extensions Time ##

Contains extension methods for registering a `TimeProvider` from the `Microsoft.Extensions.TimeProvider` framework.

## Jouska AutoFixture Extensions Vogen ##

Contains extension methods to register custom builders to allow `Vogen` types to be constructed by the
fixture.