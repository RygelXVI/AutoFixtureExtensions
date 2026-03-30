using AutoFixture.Kernel;
using Imposter.Abstractions;
using System.Reflection;

namespace Jouska.AutoFixture.AutoImposter;

public class ImposterRelay : ISpecimenBuilder
{
    private readonly List<Assembly> _testAssemblies;

    public IRequestSpecification Specification { get; }

    public ImposterRelay(IRequestSpecification specification, List<Assembly> testAssemblies)
    {
        Specification = specification ?? throw new ArgumentNullException(nameof(specification));
        _testAssemblies = testAssemblies ?? throw new ArgumentNullException(nameof(testAssemblies));
    }

    public object Create(object request, ISpecimenContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!Specification.IsSatisfiedBy(request))
            return new NoSpecimen();

        var t = request as Type;
        if (t == null)
            return new NoSpecimen();

        var result = ResolveMock(t);

        return result ?? new NoSpecimen();
    }

    private object? ResolveMock(Type t)
    {
        // in the assembly where the GenerateImposter attribute is used, there will be a generated static class with extension methods for creating imposters of the types marked with the attribute.
        // We need to find that class and invoke the appropriate method to create the imposter instance.
        // The naming convention for the generated class is {TypeName}ImposterExtensions,
        // and it contains a static method named Imposter that creates an imposter instance for the type.
        // There is also a static extension method named Instance that takes the imposter object and returns the actual imposter instance.

        // Naming Conventions
        // {(ImposterTarget.GetType().Assembly.Name}.{ImposterTarget}Imposter.Imposter(Imposter.Abstractions.ImposterMode)
        // e.g. public static global::AutoFixture.AutoImposter.TestHelpers.ICalculatorImposter Imposter(global::Imposter.Abstractions.ImposterMode invocationBehavior = global::Imposter.Abstractions.ImposterMode.Implicit) => new global::AutoFixture.AutoImposter.TestHelpers.ICalculatorImposter(invocationBehavior);

        // {(ImposterTarget.GetType().Assembly.Name}.{ImposterTarget}Imposter.Instance()
        // e.g. public static global::AutoFixture.AutoImposter.TestHelpers.ICalculatorImposter Instance(this global::AutoFixture.AutoImposter.TestHelpers.ICalculatorImposter imposter) => imposter;

        var tassemTypes = _testAssemblies
            .SelectMany(x => x.GetTypes())
            .ToList();

        var imposterExtensionsType = tassemTypes
            .Where(x => x.Name.StartsWith(t.Name) && x.Name.EndsWith("ImposterExtensions"))
            .ToList();

        var createImposterExtensionMethod = imposterExtensionsType
            .SelectMany(x => x.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .FirstOrDefault(x => x.Name.StartsWith("Imposter"));

        var imposterObj = createImposterExtensionMethod?.Invoke("null", [ImposterMode.Implicit]);

        var createImposterInstanceMethod = typeof(ImposterExtensions)
            .GetMethod("Instance", BindingFlags.Static | BindingFlags.Public)
            ?.MakeGenericMethod(t);

        var imposterInstance = createImposterInstanceMethod?.Invoke(null, [imposterObj!]);

        return imposterInstance;
    }
}
