namespace RC.CA.Application.Models;




/// <summary>
/// Marker class. Used for api calls which have no parameters. Example _httpHelper.HttpPostHelper requires a request object.
/// </summary>
public class EmptyRequest : IServiceRequest
{
    public int MyProperty { get; set; }
}
