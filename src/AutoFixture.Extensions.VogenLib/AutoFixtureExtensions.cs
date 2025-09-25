using AutoFixture.Extensions.VogenLib.Builders;
using AutoFixture.Kernel;

namespace AutoFixture.Extensions.VogenLib;

public static class AutoFixtureExtensions
{
    /// <summary>
    /// Registers a builder that will allow the fixture to a Vogen type using the "From" factory method, and supplying the default underlying type.
    /// </summary>
    /// <returns>IFixture</returns>
    public static IFixture WithDefaultVogenBuilder(this IFixture fixture)
    {
        fixture.Customizations.Add(new FilteringSpecimenBuilder(new VogenTypeSpecimenBuilder(), new VogenTypeSpecification()));
        return fixture;
    }
}
