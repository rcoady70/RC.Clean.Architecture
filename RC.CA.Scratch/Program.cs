// See https://aka.ms/new-console-template for more information
using System.Reflection;
using RC.CA.SharedKernel.Result;

try
{
    var e = new ValidationError()
    {
        ErrorCode = "123456",
        ErrorMessage = "Message",
        Severity = ValidationSeverity.Error,
        Identifier = "",
    };
    List<ValidationError> lE = new List<ValidationError>();
    lE.Add(e);
    //public static CAResult<T> Invalid(List<ValidationError> validationErrors)

    MethodInfo MI = typeof(CAResult<string>).GetMethod("Invalid", new[] { typeof(List<ValidationError>) });
    var x = MI.Invoke(null, new object[] { lE });
    Console.WriteLine("Hello");
}
catch (Exception ex)
{
    string message = ex.Message;
}


class MyClass<T>
{
    public static string Invalid(List<ValidationError> errors)
    {
        Console.WriteLine("Hello");
        return "Called......";
    }
}

