using AutoFixture.Kernel;
using Vogen;

namespace AutoFixture.Extensions.VogenLib.Builders;

public class VogenTypeSpecification : IRequestSpecification
{
    public bool IsSatisfiedBy(object request)
    {
        var requestType = request.GetType();

        if (requestType != null)
        {
            var valueObjectAttribute = requestType.GetCustomAttributes(typeof(ValueObjectAttribute), false);
            var valueObjectGenericAttribute = requestType.GetCustomAttributes(typeof(ValueObjectAttribute<>), false);

            if (valueObjectAttribute.Any() || valueObjectGenericAttribute.Any())
            {
                return true;
            }
        }

        return false;
    }
}