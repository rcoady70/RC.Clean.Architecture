using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Constants;

public static class DB
{
    public const string RoleAdmin = "Admin";
    public const string RoleUser = "User";
    //Pagination 
    public const Int16 ListItemsPerPage = 10;
    public const string OperatorIn = "in";
    public const string OperatorLike = "like";
    public const string OperatorNotEqual = "notequal";
    public const string ConditionAnd = "and";
    public const string ConditionOr = "or";
}
