using Vogen;

namespace Jouska.AutoFixture.Extensions.VogenLib.Specification;

[ValueObject<int>]
public partial class TestClass
{
    private static Validation Validate(int value)
    {
        return value > 0
            ? Validation.Ok
            : Validation.Invalid("Must be greater than zero.");
    }
    private static int NormalizeInput(int input) => input;
}
