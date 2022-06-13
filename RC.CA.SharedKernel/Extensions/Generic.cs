using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Extensions
{
    public  static class GenericExtensions
    {
        /// <summary>
        /// Check if value is part of a list, simplify checking logic. example "And".IsInExt("And","Or"))   24.IsInExt(1,2,3)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="T"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsInExt<T>(this T self,params T[] values)
        {
            return values.Contains(self);
        }
    }
}
