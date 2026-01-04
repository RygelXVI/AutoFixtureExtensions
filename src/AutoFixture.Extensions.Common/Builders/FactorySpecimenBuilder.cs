using AutoFixture.Kernel;
using System.Reflection;

namespace AutoFixture.Extensions.Common.Builders;

public class FactorySpecimenBuilder<TTarget, TFactory> : ISpecimenBuilder
    where TTarget : class
    where TFactory : class
{
    public object Create(object request, ISpecimenContext context)
    {
        var requestType = TryGetRequestType(request);

        if (requestType == typeof(TTarget))
        {
            if (context.Resolve(typeof(TFactory)) is TFactory factory)
            {
                var methodInfo = GetFactoryMethod();

                if (methodInfo != null)
                {
                    var args = ResolveFactoryParameters(methodInfo.Parameters, context);
                    var result = methodInfo.Method.Invoke(factory, args);
                    if (result is TTarget target)
                    {
                        return target;
                    }
                }
            }
        }

        return new NoSpecimen();
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

    private static FactoryMethod? GetFactoryMethod()
    {
        var typeInfo = typeof(TFactory).GetTypeInfo();

        return typeInfo.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.ReturnType.IsAssignableFrom(typeof(TTarget)))
            .Select(x => new FactoryMethod
            {
                Method = x,
                Parameters = x.GetParameters()
            })
            .MaxBy(x => x.Parameters.Length);
    }

    private static object[] ResolveFactoryParameters(ParameterInfo[] parameters, ISpecimenContext context) =>
        [.. parameters.Select(p => context.Resolve(p.ParameterType))];
}
