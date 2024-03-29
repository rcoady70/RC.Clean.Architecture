﻿namespace RC.CA.SharedKernel.Result;

public class CAResultEmpty : CAResult<CAResultEmpty>
{
    public CAResultEmpty() : base() { }

    protected internal CAResultEmpty(ResultStatus status) : base(status) { }

    /// <summary>
    /// Represents a successful operation without return type
    /// </summary>
    /// <returns>A CAResultEmpty</returns>
    public static CAResultEmpty Success()
    {
        return new CAResultEmpty();
    }

    /// <summary>
    /// Represents a successful operation without return type
    /// </summary>
    /// <param name="successMessage">Sets the SuccessMessage property</param>
    /// <returns>A CAResultEmpty></returns>
    public static CAResultEmpty SuccessWithMessage(string successMessage)
    {
        return new CAResultEmpty() { SuccessMessage = successMessage };
    }

    /// <summary>
    /// Represents a successful operation and accepts a values as the result of the operation
    /// </summary>
    /// <param name="value">Sets the Value property</param>
    /// <returns>A CAResultEmpty<typeparamref name="T"/></returns>
    public static CAResult<T> Success<T>(T value)
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
    public static CAResult<T> Success<T>(T value, string successMessage)
    {
        return new CAResult<T>(value, successMessage);
    }

    /// <summary>
    /// Represents an error that occurred during the execution of the service.
    /// Error messages may be provided and will be exposed via the Errors property.
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>
    /// <returns>A CAResultEmpty</returns>
    public static new CAResultEmpty ServerError(params string[] errorMessages)
    {
        return new CAResultEmpty(ResultStatus.ServerError) { Errors = errorMessages };
    }

    /// <summary>
    /// Represents validation errors that prevent the underlying service from completing.
    /// </summary>
    /// <param name="validationErrors">A list of validation errors encountered</param>
    /// <returns>A CAResultEmpty</returns>
    public static new CAResultEmpty Invalid(List<ValidationError> validationErrors)
    {
        return new CAResultEmpty(ResultStatus.BadRequest) { ValidationErrors = validationErrors };
    }

    /// <summary>
    /// Represents the situation where a service was unable to find a requested resource.
    /// </summary>
    /// <returns>A CAResultEmpty</returns>
    public static new CAResultEmpty NotFound()
    {
        return new CAResultEmpty(ResultStatus.NotFound);
    }

    /// <summary>
    /// The parameters to the call were correct, but the user does not have permission to perform some action.
    /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A CAResultEmpty</returns>
    public static new CAResultEmpty Forbidden()
    {
        return new CAResultEmpty(ResultStatus.Forbidden);
    }

    /// <summary>
    /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate but failed.
    /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A CAResultEmpty</returns>
    public static new CAResultEmpty Unauthorized()
    {
        return new CAResultEmpty(ResultStatus.Unauthorized);
    }
}
