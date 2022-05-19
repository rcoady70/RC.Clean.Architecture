using System.Text.RegularExpressions;
using FluentValidation;

namespace RC.CA.Application.Validators;
/// <summary>
/// Validation extensions centralize to match field types in database. 
/// example not required RuleFor(p => p.Email).ExtIsValidEmail().When(p => !string.IsNullOrEmpty(p.Email));
///             required RuleFor(p => p.Email).ExtIsValidEmail();
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// Common rule to check email address, Max length 50 not empty not null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidEmail<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
                   .NotEmpty()
                   .NotNull()
                   .EmailAddress()
                   .MaximumLength(50)
                   .WithMessage("{PropertyName} Email address is invalid");
    }
    /// <summary>
    /// Common rule for string keys must be less than 50 not null,not empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtCompanyId<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
               .NotEmpty()
               .NotNull().WithMessage("{PropertyName} cannot be blank")
               .NotEmpty().WithMessage("{PropertyName} cannot be blank")
               .NotEqual("string").WithMessage("{PropertyName} has invalid value")
               .MaximumLength(50).WithMessage("{PropertyName} must be between 2 - 50 characters in length")
               .MinimumLength(2).WithMessage("{PropertyName} must be between 2 - 50 characters in length")
               .Matches("[a-z,A-Z,0-9]").WithMessage("{PropertyName} can only contain characters and numbers \"a\" To \"z\" and \"0\" to \"9\"");
               
    }
    /// <summary>
    /// Common rule for name must be less than 50
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidName<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
                   .MaximumLength(50).WithMessage("{PropertyName} must be less than 50 characters ");
    }
    /// <summary>
    /// Common rule for name must be less than 50
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidFileName<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
                   .NotEmpty().WithMessage("{PropertyName} cannot be blank")
                   .MaximumLength(50).WithMessage("{PropertyName} must be less than 250 characters ");
        
    }
    /// <summary>
    /// Common rule for name must be less than 50
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidDescription<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
                   .MaximumLength(50).WithMessage("{PropertyName} must be less than 50 characters ");
    }
    /// <summary>
    /// Common rule for name must be less than 50
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidGender<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
                   .MaximumLength(25).WithMessage("{PropertyName} must be less than 50 characters ");
    }
    /// <summary>
    /// Simple rule to check for restricted characters 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsRestrictedChar<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(propertyValue =>
        {
            if (string.IsNullOrEmpty(propertyValue))
                return true;
            return !Regex.IsMatch(propertyValue, "[<>:]|javascript", RegexOptions.IgnoreCase);
        }).WithMessage("{PropertyName} contains restricted characters {PropertyValue}");
    }

    /// <summary>
    /// Simple rule to check for restricted characters 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidCharList<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(propertyValue =>
        {
            if (string.IsNullOrEmpty(propertyValue))
                return true;
            return Regex.IsMatch(propertyValue, @"[a-z,0-9,A-Z,@,:,\/]", RegexOptions.IgnoreCase);
        }).WithMessage("{PropertyName} contains invalid characters");
    }

    /// <summary>
    /// check filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtFilter<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
                   .MaximumLength(50).WithMessage("{PropertyName} must be between 0 - 50 characters in length")
                   .When(x => x != null, ApplyConditionTo.AllValidators);
                   
    }
    /// <summary>
    /// Common rule to validate string is a valid date
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidDate<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(RuleValidDate)
                   .WithMessage("{PropertyName} Invalid date/time {PropertyValue}");
    }
    /// <summary>
    /// Common rule to validate string is a valid date
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidFutureDate<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(RuleFutureDate)
                   .WithMessage("{PropertyName} Date must be in the future {PropertyValue}");
    }
    private static bool RuleValidDate(string value)
    {
        DateTime date;
        if (!DateTime.TryParse(value, out date))
            return false;
        
        return true;
    }
    private static bool RuleFutureDate(string? value)
    {
        DateTime date;
        if (!DateTime.TryParse(value, out date))
            return false;
        if (date < DateTime.Now)
            return false;
        return true;
    }
}


