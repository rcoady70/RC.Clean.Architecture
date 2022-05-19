using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Exceptions;
/// <summary>
/// Sort order exception
/// </summary>
public partial class SortOrderException : System.Exception
{
    public string Entity { get; private set; }
    public string Field { get; private set; }
    public string Hostname { get; private set; }

    public SortOrderException(string entity, string field, System.Exception? innerException)
                        : base($"Sort field \n{field} not found in entity {entity}\n")
    {
        Entity = entity;
        Field = field;
        Hostname = Environment.MachineName;
    }

}
