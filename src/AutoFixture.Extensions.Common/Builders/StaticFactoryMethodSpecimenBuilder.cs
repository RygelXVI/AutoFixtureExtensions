using AutoFixture.Kernel;
using System.Reflection;

namespace AutoFixture.Extensions.Common.Builders;

public class StaticFactoryMethodSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var requestType = TryGetRequestType(request);

        return requestType != null
            ? CreateSpecimen(requestType, context)
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

    private static object CreateSpecimen(Type requestType, ISpecimenContext context)
    {
        var factoryMethod = GetStaticFactoryMethods(requestType);
        return InvokeFactoryMethod(factoryMethod, context);
    }

    private static FactoryMethod? GetStaticFactoryMethods(Type requestType)
    {
        var typeInfo = requestType.GetTypeInfo();

        return typeInfo.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.ReturnType == requestType)
            .Select(x => new FactoryMethod
            {
                Method = x,
                Parameters = x.GetParameters()
            })
            .MaxBy(x => x.Parameters.Length);
    }

    private static object InvokeFactoryMethod(FactoryMethod? factoryMethod, ISpecimenContext context)
    {
        if (factoryMethod != null)
        {
            var args = ResolveFactoryParameters(factoryMethod.Parameters, context);
            var output = factoryMethod.Method.Invoke(null, args);
            if (output != null)
            {
                return output;
            }
        }

        return new NoSpecimen();
    }

    private static object[] ResolveFactoryParameters(ParameterInfo[] parameters, ISpecimenContext context) =>
        [.. parameters.Select(p => context.Resolve(p.ParameterType))];
}
