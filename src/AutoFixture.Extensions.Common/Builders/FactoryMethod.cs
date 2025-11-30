using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AutoFixture.Extensions.Common.Builders;

public partial class StaticFactoryMethodSpecimenBuilder
{
    internal class FactoryMethod
    {
        [NotNull]
        public MethodInfo? Method { get; set; }
        [NotNull]
        public ParameterInfo[]? Parameters { get; set; }
    }
}
