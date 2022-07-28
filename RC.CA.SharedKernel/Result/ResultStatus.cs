namespace RC.CA.SharedKernel.Result;

public enum ResultStatus
{
    Ok = 200,
    ServerError = 500,
    Forbidden = 403,
    Unauthorized = 401,
    BadRequest = 400,
    NotFound = 404
}
