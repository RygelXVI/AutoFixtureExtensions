using Vogen;

namespace Jouska.AutoFixture.Extensions.VogenLib.Specification;

[ValueObject]
public partial class TestClass2
{
    private static Validation Validate(int value)
    {
        return value > 0
            ? Validation.Ok
            : Validation.Invalid("Must be greater than zero.");
    }
    private static int NormalizeInput(int input) => input;
}