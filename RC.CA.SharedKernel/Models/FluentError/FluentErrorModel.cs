using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Models.FluentError;
/// <summary>
/// Fluent error model this can be returned from api if fluent validation fails before api is called
/// </summary>
public class FluentErrorModel
{
    public Dictionary<string, string[]> errors { get; set; }
    public string type { get; set; }
    public string title { get; set; }
    public int status { get; set; }
    public string traceId { get; set; }
}
/// <summary>
/// Fluent error collection
/// </summary>
public class FluentError
{
    public string[] _ { get; set; }
}
