using AutoFixture.Kernel;

namespace AutoFixture.Extensions.Common;

public static class AutoFixtureExtensions
{
    public static IFixture Inject<TInterface, TImplementation>(this IFixture fixture, TImplementation implementation) where TImplementation : class, TInterface
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TInterface), typeof(TImplementation)));
        fixture.Register(()  => implementation);

        return fixture;
    }

    public static IFixture WithTypeRelay<TInterface, TImplementation>(this IFixture fixture)
    {
        fixture.Customizations.Add(new TypeRelay(typeof(TInterface), typeof(TImplementation)));
        return fixture;
    }

    public static IFixture WithNamedParameterValue<TParam>(this IFixture fixture, string parameterName, TParam value)
    {
        var parameterSpecification = new ParameterSpecification(typeof(TParam), parameterName);
        var fixedBuilder = new FixedBuilder(value);
        var filteringSpecimenBuilder = new FilteringSpecimenBuilder(fixedBuilder, parameterSpecification);
        fixture.Customizations.Add(filteringSpecimenBuilder);
        return fixture;
    }

}
