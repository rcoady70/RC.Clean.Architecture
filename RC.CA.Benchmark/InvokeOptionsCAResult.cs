using System.Reflection;
using BenchmarkDotNet.Attributes;
using RC.CA.SharedKernel.Result;

namespace RC.CA.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class InvokeOptionsCAResult
    {
        [Benchmark]
        public void CompareDirectInvocation()
        {
            List<RC.CA.SharedKernel.Result.ValidationError> errorList = new List<RC.CA.SharedKernel.Result.ValidationError>();
            errorList.Add(new RC.CA.SharedKernel.Result.ValidationError()
            {
                ErrorCode = "12345",
                ErrorMessage = "",
                Identifier = "",
                Severity = SharedKernel.Result.ValidationSeverity.Info
            });
            MethodInfo methodInfo = typeof(CAResult<SampleObject>).GetMethod("Invalid", new[] { typeof(List<RC.CA.SharedKernel.Result.ValidationError>) });
            //Execute none generic static method to create invalid CAResult<t> 
            var errorResponse = (CAResult<SampleObject>)methodInfo.Invoke(null, new object[] { errorList });
        }
        [Benchmark]
        public void CompareNewUp()
        {
            List<RC.CA.SharedKernel.Result.ValidationError> errorList = new List<RC.CA.SharedKernel.Result.ValidationError>();
            errorList.Add(new RC.CA.SharedKernel.Result.ValidationError()
            {
                ErrorCode = "12345",
                ErrorMessage = "",
                Identifier = "",
                Severity = SharedKernel.Result.ValidationSeverity.Info
            });

            var errorResponse = CAResult<SampleObject>.Invalid(errorList);
        }

    }
    public class SampleObject
    {
        public string xx = "";
        public int yy = 0;
    }
}
