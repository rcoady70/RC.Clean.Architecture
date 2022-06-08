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
        return rule.NotEmpty()
                   .NotNull()
                   .EmailAddress()
                   .MaximumLength(50).WithMessage("{PropertyName} Email must be less than 50 characters")
                   .WithMessage("{PropertyName} Email address is invalid");
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
        return rule.Matches(ValidationRegex.WhiteListForName).WithMessage("{PropertyName} contains invalid characters")
                   .MaximumLength(50).WithMessage("{PropertyName} Email must be less than 50 characters");
    }
    /// <summary>
    /// Common rule for URI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <param name="required"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtIsValidUri<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.Must(uri =>
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out _);
        }).WithMessage("{PropertyName} Uri is not valid");
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
        var m = Regex.IsMatch("", ValidationRegex.WhiteListForDescription, RegexOptions.IgnoreCase);
        return rule.ExtIsRestrictedChar()
                   .MaximumLength(50).WithMessage("{PropertyName} must be less than 50 characters ")
                   .Matches(ValidationRegex.WhiteListForDescription).WithMessage("{PropertyName} contains invalid characters");
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
                   .Matches(@"^[M,F,U]").WithMessage("{PropertyName} Gender is invalid ");
                   
    }
    /// <summary>
    /// Simple rule to check for restricted characters just simple layer does not take into account escaped values. 
    /// All input should be WHITELISTED
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
    /// Check list filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ExtFilter<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.ExtIsRestrictedChar()
                   .Matches(ValidationRegex.WhiteListForListFilter).WithMessage("{PropertyName} contains invalid characters")
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


