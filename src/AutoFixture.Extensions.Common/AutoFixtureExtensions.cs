using AutoFixture.Kernel;

namespace AutoFixture.Extensions.Common;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Registers an instance of a type, together with it's interface
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="fixture"></param>
    /// <param name="implementation"></param>
    /// <returns>IFixture</returns>
    public static IFixture Inject<TInterface, TImplementation>(this IFixture fixture, TImplementation implementation) where TImplementation : class, TInterface
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TInterface), typeof(TImplementation)));
        fixture.Register(()  => implementation);

        return fixture;
    }

    /// <summary>
    /// Registers a type relay, allowing requests for an interface or base class to be forwarded to a registered implementation.<br/>
    /// Useful when a type implements multiple interfaces (e.g. IDisposable).
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    /// <param name="fixture"></param>
    /// <returns>IFixture</returns>
    public static IFixture WithTypeRelay<TInterface, TImplementation>(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TInterface), typeof(TImplementation)));
        return fixture;
    }

    /// <summary>
    /// Registers a specific value to be used for any matching parameter. Parameter must match in both type and name.
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    /// <param name="fixture"></param>
    /// <param name="parameterName"></param>
    /// <param name="value"></param>
    /// <returns>IFixture</returns>
    public static IFixture WithNamedParameterValue<TParam>(this IFixture fixture, string parameterName, TParam value)
    {
        var parameterSpecification = new ParameterSpecification(typeof(TParam), parameterName);
        var fixedBuilder = new FixedBuilder(value);
        var filteringSpecimenBuilder = new FilteringSpecimenBuilder(fixedBuilder, parameterSpecification);
        fixture.Customizations.Add(filteringSpecimenBuilder);
        return fixture;
    }

}
