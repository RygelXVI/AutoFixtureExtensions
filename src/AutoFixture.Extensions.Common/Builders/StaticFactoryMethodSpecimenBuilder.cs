using AutoFixture.Kernel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoFixture.Extensions.Common.Builders;

public class StaticFactoryMethodSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type typeRequest)
        {
            var typeInfo = typeRequest.GetTypeInfo();

            var factoryMethod = GetStaticFactoryMethods(typeInfo);

            if (factoryMethod != null)
            {
                var args = ResolveFactoryParameters(factoryMethod.Parameters, context);

                var output = factoryMethod.Method.Invoke(null, args);

                if (output != null)
                {
                    return output;
                }
            }
        }

        return new NoSpecimen();
    }

    private static object[] ResolveFactoryParameters(ParameterInfo[] parameters, ISpecimenContext context) => 
        [.. parameters.Select(p => context.Resolve(p.ParameterType))];

    private static FactoryMethod? GetStaticFactoryMethods(TypeInfo requestType) => 
        requestType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(x => x.ReturnType == requestType)
            .Select(x => new FactoryMethod
            {
                Method = x,
                Parameters = x.GetParameters()
            })
            .MaxBy(x => x.Parameters.Length);

    internal class FactoryMethod
    {
        [NotNull]
        public MethodInfo? Method { get; set; }
        [NotNull]
        public ParameterInfo[]? Parameters { get; set; }
    }
}
