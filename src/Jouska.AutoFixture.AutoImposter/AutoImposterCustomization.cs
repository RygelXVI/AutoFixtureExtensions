using AutoFixture;
using AutoFixture.Kernel;
using Imposter.Abstractions;
using System.Reflection;

namespace Jouska.AutoFixture.AutoImposter;

public class AutoImposterCustomization : ICustomization
{
    private readonly HashSet<Type> _mockableTypes;
    private readonly IRequestSpecification _mockableSpecification;

    public AutoImposterCustomization(IEnumerable<Assembly> assemblies)
    {
        _mockableTypes = GetMockableTypes(assemblies);
        _mockableSpecification = new IsMockableSpecification(_mockableTypes);
        SpecimenBuilder = new ImposterRelay(_mockableSpecification, [.. assemblies]);
    }

    public ISpecimenBuilder SpecimenBuilder { get; set; }

    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(SpecimenBuilder);
    }

    private static HashSet<Type> GetMockableTypes(IEnumerable<Assembly> assemblies)
    {
        var mockableTypes = assemblies
            .SelectMany(x => x.CustomAttributes.Where(a => a.AttributeType == typeof(GenerateImposterAttribute)))
            .Select(a => a.ConstructorArguments[0].Value as Type)
            .Where(t => t != null)
            .ToList();
            
        var result = mockableTypes != null 
            ? new HashSet<Type>(mockableTypes!) 
            : [];

        return result;        
    }
}
