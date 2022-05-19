using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Infrastructure.Persistence.EntityConfig;

public static class ColumnTypes
{
    public const string EmailCol150 = "nvarchar(150)"; //Email address 
    public const string Name50 = "nvarchar(50)"; //String as primary key
    public const string FileName250 = "nvarchar(250)"; //File name 
    public const string DescriptionCol50 = "nvarchar(50)"; //Standard description
    public const string URLCol250 = "nvarchar(250)"; //URL
    public const string NameCol25 = "nvarchar(25)"; //Standard description
    public const string UserCol50 = "nvarchar(50)"; //Standard 50 nvarchar
    public const string UserCol100 = "nvarchar(100)"; //Standard 50 nvarchar
    public const string IPCol50 = "nvarchar(50)"; //Standard 50 nvarchar
    public const string TextCol = "nText";
    public const string Guid = "uniqueidentifier";
    public const string BigInt = "bigint";
}
