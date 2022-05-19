using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.ValueTypes;
/// <summary>
/// Define Entity Audit fields
/// </summary>
/// <typeparam name="TId"></typeparam>
public class EntityKeyTennant<TId>
{
    public DateTime DateTime { get; set; }
    public int UserId { get; set; } = default!;
    public string UserName { get; set; } = default!;
}
