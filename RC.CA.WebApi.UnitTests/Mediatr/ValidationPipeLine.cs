using System.Reflection;
using RC.CA.SharedKernel.Result;

namespace RC.CA.WebApi.UnitTests.Mediatr
{
    [Trait("Category", "Mediatr")]
    public class ValidationPipeLine
    {

        [Fact]
        public async Task CAResult_CreateCAResultInvalidUsingReflection_Ok()
        {
            var error = new ValidationError()
            {
                ErrorCode = "123456",
                ErrorMessage = "Error message",
                Severity = ValidationSeverity.Error,
                Identifier = "",
            };
            List<ValidationError> errorList = new List<ValidationError>();
            errorList.Add(error);


            //Create CAResult<T>.Invalid(ErrorList)
            MethodInfo methodInfo = typeof(CAResult<string>).GetMethod("Invalid", new[] { typeof(List<ValidationError>) });
            //Execute none generic method to create invalid CA
            var errorResponse = (CAResult<string>)methodInfo.Invoke(null, new object[] { errorList });

            Assert.Equal(errorResponse.ValidationErrors.Count, 1);
            Assert.Equal(errorResponse.IsSuccess, false);
            Assert.Equal(errorResponse.ValidationErrors[0].ErrorCode, "123456");
            Assert.Equal(errorResponse.ValidationErrors[0].ErrorMessage, "Error message");
            Assert.Equal(errorResponse.ValidationErrors[0].Severity, ValidationSeverity.Error);
        }

    }
}
