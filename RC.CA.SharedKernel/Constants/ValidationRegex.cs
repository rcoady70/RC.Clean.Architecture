using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Constants
{
    public static class ValidationRegex
    {
        public const string WhiteListForName = @"^[\w-'\. ]{0,50}$";
        public const string WhiteListForDescription = @"^[\w-_ ]{0,50}$";
        public const string WhiteListForListFilter = @"^[\w-_\*@\.' ]{0,50}$";
    }
}
