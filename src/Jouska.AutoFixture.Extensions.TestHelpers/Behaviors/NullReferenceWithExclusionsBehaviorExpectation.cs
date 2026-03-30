using AutoFixture.Idioms;
using System.Globalization;

namespace Jouska.AutoFixture.Extensions.TestHelpers.Behaviors;

public class NullReferenceWithExclusionsBehaviorExpectation : IBehaviorExpectation
{
    private readonly string[] _excludedParameters;

    public NullReferenceWithExclusionsBehaviorExpectation(params string[] excludedParameters)
    {
        _excludedParameters = excludedParameters ?? throw new ArgumentNullException(nameof(excludedParameters));
    }

    public void Verify(IGuardClauseCommand command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        if (_excludedParameters.Contains(command.RequestedParameterName))
        {
            return;
        }

        if (!command.RequestedType.IsClass &&
            !command.RequestedType.IsInterface)
        {
            return;
        }

        try
        {
            command.Execute(null);
        }
        catch (ArgumentNullException ex)
        {
            if (string.Equals(ex.ParamName, command.RequestedParameterName, StringComparison.Ordinal))
            {
                return;
            }

            throw command.CreateException(
                "<null>",
                string.Format(CultureInfo.InvariantCulture,
                    "Guard Clause prevented it, however the thrown exception contains invalid parameter name. " +
                    "Ensure you pass correct parameter name to the ArgumentNullException constructor." +
                    "{0}Expected parameter name: {1}{0}Actual parameter name: {2}",
                    Environment.NewLine,
                    command.RequestedParameterName,
                    ex.ParamName),
                ex);
        }
        catch (Exception ex)
        {
            throw command.CreateException("null", ex);
        }

        throw command.CreateException("null");
    }
}