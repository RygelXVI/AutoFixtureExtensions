using AutoFixture.Idioms;
using System.Globalization;

namespace AutoFixture.Extensions.TestHelpers.Behaviors;

public class InvalidArgumentByNameBehaviorExpectation<T> : IBehaviorExpectation
{
    private readonly T _invalidArgument;
    private readonly string _argumentName;

    public InvalidArgumentByNameBehaviorExpectation(
        T invalidArgument,
        string argumentName)
    {
        _invalidArgument = invalidArgument;
        _argumentName = argumentName;
    }

    public void Verify(IGuardClauseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.RequestedParameterName != _argumentName)
        {
            return;
        }

        try
        {
            command.Execute(_invalidArgument);
        }
        catch (ArgumentException ex)
        {
            if (string.Equals(ex.ParamName, command.RequestedParameterName, StringComparison.Ordinal))
            {
                return;
            }

            throw command.CreateException(
                _argumentName,
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
            throw command.CreateException(_argumentName, ex);
        }

        throw command.CreateException(_argumentName);
    }
}
