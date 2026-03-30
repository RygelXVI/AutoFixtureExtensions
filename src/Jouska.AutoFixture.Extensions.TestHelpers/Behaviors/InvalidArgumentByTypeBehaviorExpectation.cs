using AutoFixture.Idioms;
using System.Globalization;

namespace Jouska.AutoFixture.Extensions.TestHelpers.Behaviors;

public class InvalidArgumentByTypeBehaviorExpectation<T> : IBehaviorExpectation
{
    private readonly T _invalidValue;

    public InvalidArgumentByTypeBehaviorExpectation(T invalidValue)
    {
        _invalidValue = invalidValue ?? throw new ArgumentNullException(nameof(invalidValue));
    }

    public void Verify(IGuardClauseCommand command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (command.RequestedType != typeof(T))
        {
            return;
        }

        try
        {
            command.Execute(_invalidValue);
        }
        catch (ArgumentException ex)
        {
            if (string.Equals(ex.ParamName, command.RequestedParameterName, StringComparison.Ordinal))
            {
                return;
            }

            throw command.CreateException(
                _invalidValue?.ToString(),
                string.Format(CultureInfo.InvariantCulture,
                    "Guard Clause prevented it, however the thrown exception contains invalid parameter name. " +
                    "Ensure you pass correct parameter name to the ArgumentException constructor.{0}" +
                    "Expected parameter name: {1}{0}Actual parameter name: {2}",
                    Environment.NewLine,
                    command.RequestedParameterName,
                    ex.ParamName),
                ex);
        }
        catch (Exception ex)
        {
            throw command.CreateException(_invalidValue?.ToString(), ex);
        }

        throw command.CreateException(_invalidValue?.ToString());
    }
}
