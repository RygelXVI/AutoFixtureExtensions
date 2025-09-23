using AutoFixture.Extensions.VogenLib.Builders;
using AutoFixture.Kernel;

namespace AutoFixture.Extensions.VogenLib;

public static class AutoFixtureExtensions
{
    public static IFixture WithDefaultVogenBuilder(this IFixture fixture)
    {
        fixture.Customizations.Add(new FilteringSpecimenBuilder(new VogenTypeSpecimenBuilder(), new VogenTypeSpecification()));
        return fixture;
    }
}
