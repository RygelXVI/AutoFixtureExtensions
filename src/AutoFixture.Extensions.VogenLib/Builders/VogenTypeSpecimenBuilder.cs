using AutoFixture.Kernel;
using System.Reflection;

namespace AutoFixture.Extensions.VogenLib.Builders;

public class VogenTypeSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is SeededRequest seededRequest && seededRequest.Request is Type typeRequest)
        {
            var requestType = typeRequest.GetTypeInfo();
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
        }

        return new NoSpecimen();
    }
}
