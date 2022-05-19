using System.Dynamic;
using System.Reflection;
using RC.CA.SharedKernel.GuardClauses;

namespace RC.CA.SharedKernel.Extensions;

public static class DateTimeExtensions
{

    /// <summary>
    /// Return blank if date is min/max/null
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToStringBlankExt(this DateTime? dateTime)
    {
        if (dateTime ==null)
            return "";

        if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
            return "";
        else
            return dateTime?.ToString();
    }
}



