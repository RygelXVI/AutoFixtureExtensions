using AutoFixture.Kernel;

namespace Jouska.AutoFixture.AutoImposter;

public class IsMockableSpecification : IRequestSpecification
{
    private readonly HashSet<Type> _mockableTypes;

    public IsMockableSpecification(IEnumerable<Type> mockableTypes)
    {
        _mockableTypes= [.. mockableTypes];
    }

    public bool IsSatisfiedBy(object request)
    {
        var t = request as Type;
        return t != null && _mockableTypes.Contains(t);
    }
}
