using AutoFixture.Idioms;
using AutoFixture.Kernel;

namespace AutoFixture.Extensions.TestHelpers.Behaviors;

public class EqualityAssertion : CompositeIdiomaticAssertion
{
    public EqualityAssertion(ISpecimenBuilder builder) : base(CreateChildrenAssertions(builder)) { }
    private static IEnumerable<IIdiomaticAssertion> CreateChildrenAssertions(ISpecimenBuilder builder)
    {
        yield return new EqualsNewObjectAssertion(builder);
        yield return new EqualsNewObjectAssertion(builder);
        yield return new EqualsSelfAssertion(builder);
        yield return new EqualsSuccessiveAssertion(builder);
        yield return new GetHashCodeSuccessiveAssertion(builder);
    }
}