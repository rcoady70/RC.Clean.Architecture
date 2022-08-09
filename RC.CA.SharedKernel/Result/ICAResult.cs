//Based on Ardalis.CAResultEmpty

namespace RC.CA.SharedKernel.Result;

public interface ICAResult
{
    ResultStatus Status { get; }
    bool IsCachedResult { get; set; }
    IEnumerable<string> Errors { get; }
    List<ValidationError> ValidationErrors { get; }
    Type ValueType { get; }
    Object GetValue();
}
