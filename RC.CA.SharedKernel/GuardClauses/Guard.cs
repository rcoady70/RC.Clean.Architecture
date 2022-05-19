using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.GuardClauses;
/// <summary>
/// Interface to provide a generic mechanism to build guard clause extension methods from.
/// </summary>
public interface IGuardClause
{
}
/// <summary>
/// Entry to guard clause
/// </summary>
public class Guard: IGuardClause
{
    private Guard() { }
    public static IGuardClause Against { get; } = new Guard();
}
