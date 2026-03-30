using AutoFixture.Kernel;
using System.Reflection;

namespace Jouska.AutoFixture.Extensions.VogenLib.Builders;

public class VogenTypeSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var requestType = TryGetRequestType(request);

        return requestType != null
            ? CreateVogenType(requestType, context)
            : new NoSpecimen();
    }

    private static Type? TryGetRequestType(object request)
    {
        if (request is Type typeRequest)
        {
            return typeRequest;
        }

        if (request is SeededRequest seededRequest && seededRequest.Request is Type seededTypeRequest)
        {
            return seededTypeRequest;
        }

        return null;
    }

    private static object CreateVogenType(Type type, ISpecimenContext context)
    {
        var requestType = type.GetTypeInfo();
        var fromMethod = requestType.GetMethod("From");

        if (fromMethod != null)
        {
            var fromParameter = fromMethod.GetParameters()[0];
            var input = context.Resolve(fromParameter.ParameterType);
            var output = fromMethod.Invoke(null, [input]);

            if (output != null)
            {
                return output;
            }
        }

        return new NoSpecimen();
    }
}
