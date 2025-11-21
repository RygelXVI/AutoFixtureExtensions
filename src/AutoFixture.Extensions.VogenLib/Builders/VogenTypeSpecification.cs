using AutoFixture.Kernel;
using System.Reflection;
using Vogen;

namespace AutoFixture.Extensions.VogenLib.Builders;

public class VogenTypeSpecification : IRequestSpecification
{
    public bool IsSatisfiedBy(object request)
    {
        if (request is SeededRequest seededRequest && seededRequest.Request is Type typeRequest)
        {            
            var requestType = typeRequest.GetTypeInfo();

            if (requestType != null)
            {
                var valueObjectAttribute = requestType.GetCustomAttributes(typeof(ValueObjectAttribute), false);
                var valueObjectGenericAttribute = requestType.GetCustomAttributes(typeof(ValueObjectAttribute<>), false);

                if (valueObjectAttribute.Any() || valueObjectGenericAttribute.Any())
                {
                    return true;
                }
            }
        }

        return false;
    }
}