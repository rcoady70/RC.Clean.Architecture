using FluentValidation;
using FluentValidation.Results;

namespace RC.CA.SharedKernel.Result;

public static class FluentValidationResultExtensions
{
    /// <summary>
    /// Convert errors to model state errors
    /// </summary>
    /// <param name="valResult"></param>
    /// <returns></returns>
    public static List<ValidationError> AsModelStateErrors(this ValidationResult valResult)
    {
        var resultErrors = new List<ValidationError>();

        foreach (var valFailure in valResult.Errors)
        {
            resultErrors.Add(new ValidationError()
            {
                Severity = FromSeverity(valFailure.Severity),
                ErrorMessage = valFailure.ErrorMessage,
                ErrorCode = valFailure.ErrorCode,
                Identifier = valFailure.PropertyName
            });
        }

        return resultErrors;
    }
    /// <summary>
    /// Convert errors to model state errors
    /// </summary>
    /// <param name="valResult"></param>
    /// <returns></returns>
    public static List<ValidationError> ToCAResultErrors(this List<FluentValidation.Results.ValidationFailure> valResult)
    {
        var resultErrors = new List<ValidationError>();

        foreach (var valFailure in valResult)
        {
            resultErrors.Add(new ValidationError()
            {
                Severity = FromSeverity(valFailure.Severity),
                ErrorMessage = valFailure.ErrorMessage,
                ErrorCode = valFailure.ErrorCode,
                Identifier = valFailure.PropertyName
            });
        }

        return resultErrors;
    }

    public static ValidationSeverity FromSeverity(Severity severity)
    {
        switch (severity)
        {
            case Severity.Error: return ValidationSeverity.Error;
            case Severity.Warning: return ValidationSeverity.Warning;
            case Severity.Info: return ValidationSeverity.Info;
            default: throw new ArgumentOutOfRangeException(nameof(severity), "Unexpected Severity");
        }
    }
}
