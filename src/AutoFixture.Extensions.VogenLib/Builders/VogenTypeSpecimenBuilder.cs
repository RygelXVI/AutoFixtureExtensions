using AutoFixture.Kernel;

namespace AutoFixture.Extensions.VogenLib.Builders;

public class VogenTypeSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is SeededRequest seededRequest)
        {
            var requestType = seededRequest.Request.GetType();
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
