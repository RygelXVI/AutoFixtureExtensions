using AutoFixture.Idioms;
using AutoFixture.Kernel;

namespace Jouska.AutoFixture.Extensions.TestHelpers.Behaviors;

public class FullEqualityComparerAssertion : CompositeIdiomaticAssertion
{
    public FullEqualityComparerAssertion(ISpecimenBuilder builder) : base(CreateChildrenAssertions(builder)) { }
    private static IEnumerable<IIdiomaticAssertion> CreateChildrenAssertions(ISpecimenBuilder builder)
    {
        yield return new EqualsNewObjectAssertion(builder);
        yield return new EqualsSelfAssertion(builder);
        yield return new EqualsSuccessiveAssertion(builder);
        yield return new GetHashCodeSuccessiveAssertion(builder);
        yield return new EqualityComparerGetHashCodeAssertion(builder);
        yield return new EqualityComparerEqualsTransitiveAssertion(builder);
        yield return new EqualityComparerEqualsSelfAssertion(builder);
        yield return new EqualityComparerEqualsNullNullAssertion(builder);
        yield return new EqualityComparerEqualsNullAssertion(builder);
        yield return new EqualityComparerEqualsSymmetricAssertion(builder);
    }
}
