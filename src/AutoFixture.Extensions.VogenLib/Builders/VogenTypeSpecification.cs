using AutoFixture.Kernel;
using System.Reflection;
using Vogen;

namespace AutoFixture.Extensions.VogenLib.Builders;

public class VogenTypeSpecification : IRequestSpecification
{
    public bool IsSatisfiedBy(object request)
    {
        if (request is Type typeRequest)
        {
            return HasVogenAttribute(typeRequest);
        }

        if (request is SeededRequest seededRequest && seededRequest.Request is Type seededRequestType)
        {            
            return HasVogenAttribute(seededRequestType);
        }

        return false;
    }

    private static bool HasVogenAttribute(Type typeRequest)
    {
        var typeInfo = typeRequest.GetTypeInfo();

        if (typeInfo != null)
        {
            var valueObjectAttribute = typeInfo.GetCustomAttributes(typeof(ValueObjectAttribute), false);
            var valueObjectGenericAttribute = typeInfo.GetCustomAttributes(typeof(ValueObjectAttribute<>), false);

            if (valueObjectAttribute.Any() || valueObjectGenericAttribute.Any())
            {
                return true;
            }
        }

        return false;
    }
}