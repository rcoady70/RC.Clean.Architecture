//Based on Ardalis.CAEmptyResult

namespace RC.CA.SharedKernel.Result;

public interface ICAResult
{
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    List<ValidationError> ValidationErrors { get; }
    Type ValueType { get; }
    Object GetValue();
}
