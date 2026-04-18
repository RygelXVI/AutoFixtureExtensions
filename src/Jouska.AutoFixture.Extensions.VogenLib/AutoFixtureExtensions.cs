using AutoFixture;
using AutoFixture.Kernel;
using Jouska.AutoFixture.Extensions.VogenLib.Builders;

namespace Jouska.AutoFixture.Extensions.VogenLib;

public static class AutoFixtureExtensions
{
    extension(IFixture fixture)
    {
        /// <summary>
        /// Registers a builder that will allow the fixture to a Vogen type using the "From" factory method, and supplying the default underlying type.
        /// </summary>
        /// <returns>IFixture</returns>
        public IFixture WithDefaultVogenBuilder()
        {
            fixture.Customizations.Add(new FilteringSpecimenBuilder(new VogenTypeSpecimenBuilder(), new VogenTypeSpecification()));
            return fixture;
        }
    }
}
