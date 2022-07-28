using System.Text.Json.Serialization;

namespace RC.CA.SharedKernel.Result;

public class CAResult<T> : ICAResult
{
    protected CAResult()
    {
        string mm = "";
    }

    /// <summary>
    /// Return result with value of T
    /// </summary>
    /// <param name="value"></param>
    public CAResult(T value)
    {
        Value = value;
        if (Value != null)
        {
            ValueType = Value.GetType();
        }
    }

    protected internal CAResult(T value, string successMessage) : this(value)
    {
        SuccessMessage = successMessage;
    }

    protected CAResult(ResultStatus status)
    {
        Status = status;
    }

    public T Value { get; }
    [JsonIgnore]
    public Type ValueType { get; private set; }
    [JsonInclude] // Causes the protected setter to be called on de-serialization.
    public ResultStatus Status { get; protected set; } = ResultStatus.Ok;
    public bool IsSuccess => Status == ResultStatus.Ok;
    [JsonInclude] // Causes the protected setter to be called on de-serialization.
    public string SuccessMessage { get; protected set; } = string.Empty;
    [JsonInclude] // Causes the protected setter to be called on de-serialization.
    public IEnumerable<string> Errors { get; protected set; } = new List<string>();
    [JsonInclude] // Causes the protected setter to be called on de-serialization.
    public List<ValidationError> ValidationErrors { get; protected set; } = new List<ValidationError>();

    /// <summary>
    /// Implicit convert TO T FROM CAResultEmpty<T>
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator T(CAResult<T> result) => result.Value;
    /// <summary>
    /// implicit convert TO CAResultEmpty<T> FROM T
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator CAResult<T>(T value) => new CAResult<T>(value);
    /// <summary>
    /// implicit convert TO CAResultEmpty<T> FROM T CAResultEmpty
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator CAResult<T>(CAResultEmpty result)
    {
        return new CAResult<T>(default(T))
        {
            Status = result.Status,
            Errors = result.Errors,
            SuccessMessage = result.SuccessMessage,
            ValidationErrors = result.ValidationErrors,
        };
    }
    /// <summary>
    /// Add an error to the validation error collection
    /// </summary>
    /// <param name="ErrorCode"></param>
    /// <param name="ErrorMessage"></param>
    /// <param name="Severity"></param>
    /// <param name="Identifier"></param>

    public void AddValidationError(string ErrorCode, string ErrorMessage, ValidationSeverity Severity, string Identifier = "")
    {
        ValidationErrors.Add(new ValidationError()
        {
            ErrorCode = ErrorCode,
            ErrorMessage = ErrorMessage,
            Severity = Severity,
            Identifier = Identifier,
        }
        );
        if (Severity == ValidationSeverity.Error && Status == ResultStatus.Ok)
            Status = ResultStatus.BadRequest;
    }

    public void ClearValueType() => ValueType = null;

    /// <summary>
    /// Returns the current value.
    /// </summary>
    /// <returns></returns>
    public object GetValue()
    {
        return this.Value;
    }

    /// <summary>
    /// Converts PagedInfo into a PagedResult<typeparamref name="T"/>
    /// </summary>
    /// <param name="pagedInfo"></param>
    /// <returns></returns>
    public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
    {
        var pagedResult = new PagedResult<T>(pagedInfo, Value)
        {
            Status = Status,
            SuccessMessage = SuccessMessage,
            Errors = Errors,
            ValidationErrors = ValidationErrors
        };

        return pagedResult;
    }

    /// <summary>
    /// Represents a successful operation and accepts a values as the result of the operation
    /// </summary>
    /// <param name="value">Sets the Value property</param>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Success(T value)
    {
        return new CAResult<T>(value);
    }

    /// <summary>
    /// Represents a successful operation and accepts a values as the result of the operation
    /// Sets the SuccessMessage property to the provided value
    /// </summary>
    /// <param name="value">Sets the Value property</param>
    /// <param name="successMessage">Sets the SuccessMessage property</param>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Success(T value, string successMessage)
    {
        return new CAResult<T>(value, successMessage);
    }

    /// <summary>
    /// Represents an error that occurred during the execution of the service.
    /// Error messages may be provided and will be exposed via the Errors property.
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> ServerError(params string[] errorMessages)
    {
        return new CAResult<T>(ResultStatus.ServerError) { Errors = errorMessages };
    }

    /// <summary>
    /// Represents validation errors that prevent the underlying service from completing.
    /// </summary>
    /// <param name="validationErrors">A list of validation errors encountered</param>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Invalid(List<ValidationError> validationErrors)
    {
        var result = new CAResult<T>(ResultStatus.BadRequest) { ValidationErrors = new List<ValidationError>() };
        foreach (var error in validationErrors)
            result.AddValidationError(error.ErrorCode, error.ErrorMessage, error.Severity, error.Identifier);
        return result;
    }
    /// <summary>
    /// Represents validation errors that prevent the underlying service from completing.
    /// </summary>
    /// <returns></returns>
    public static CAResult<T> Invalid()
    {
        var result = new CAResult<T>(ResultStatus.BadRequest) { ValidationErrors = new List<ValidationError>() };
        return result;
    }
    /// <summary>
    /// Represents validation error that prevent the underlying service from completing.
    /// </summary>
    /// <param name="validationErrors">A list of validation errors encountered</param>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Invalid(ValidationError validationError)
    {
        var result = new CAResult<T>(ResultStatus.BadRequest) { ValidationErrors = new List<ValidationError>() };
        result.AddValidationError(validationError.ErrorCode, validationError.ErrorMessage, validationError.Severity, validationError.Identifier);
        return result;
    }
    /// <summary>
    /// Represents validation error that prevent the underlying service from completing.
    /// </summary>
    /// <param name="validationErrors">A list of validation errors encountered</param>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Invalid(string errorCode, string errorMessage, ValidationSeverity severity, string identifier = "")
    {
        var result = new CAResult<T>(ResultStatus.BadRequest) { ValidationErrors = new List<ValidationError>() };
        result.AddValidationError(errorCode, errorMessage, severity, identifier);
        return result;
    }

    /// <summary>
    /// Represents the situation where a service was unable to find a requested resource.
    /// </summary>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> NotFound()
    {
        return new CAResult<T>(ResultStatus.NotFound);
    }

    /// <summary>
    /// The parameters to the call were correct, but the user does not have permission to perform some action.
    /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Forbidden()
    {
        return new CAResult<T>(ResultStatus.Forbidden);
    }

    /// <summary>
    /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed.
    /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Unauthorized()
    {
        return new CAResult<T>(ResultStatus.Unauthorized);
    }
    /// <summary>
    /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed with error. 
    /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Unauthorized(string errorCode, string errorMessage, ValidationSeverity severity, string identifier = "")
    {
        var result = new CAResult<T>(ResultStatus.Unauthorized) { ValidationErrors = new List<ValidationError>() };
        result.AddValidationError(errorCode, errorMessage, severity, identifier);
        return result;
    }
}
