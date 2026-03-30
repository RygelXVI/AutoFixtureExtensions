using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Jouska.AutoFixture.Extensions.Common.Builders;

internal class FactoryMethod
{
    [NotNull]
    public MethodInfo? Method { get; set; }
    [NotNull]
    public ParameterInfo[]? Parameters { get; set; }
}
