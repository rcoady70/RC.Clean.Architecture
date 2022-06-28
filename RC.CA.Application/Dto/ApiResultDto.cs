namespace RC.CA.Application.Dto
{
    /// <summary>
    /// Alternative way to wrap result instead of inheriting. Alt to (BaseResponseDto)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultDto<T>
    {
        /// <summary>
        /// Return successful eg. Result<ActivityDto>.Success(activity);
        /// </summary>
        ///<param name="result"></param>
        /// <returns></returns>
        public static ApiResultDto<T> Success(T value)
        {
            return new ApiResultDto<T> { IsSuccess = true, Result = value };
        }
        /// <summary>
        /// Return successful eg. Result<ActivityDto>.Failure(activity);
        /// </summary>
        ///<param name="result"></param>
        /// <returns></returns>
        public static ApiResultDto<T> Failure(string error)
        {
            return new ApiResultDto<T> { IsSuccess = false, Error = error };
        }
        public T? Result { get; private set; } = default;
        public bool IsSuccess { get; private set; } = false;
        /// <summary>
        /// Error message
        /// </summary>
        public string Error { get; private set; }

    }
}
