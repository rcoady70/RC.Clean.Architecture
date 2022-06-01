using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Constants;

public static class WebConstants
{
    public const string HttpFactoryName = "WebApiHttp";
    public const string CorrelationId = "X-Correlation-ID";
    public const string CORSPolicyName = "Internal";
    public const string EventBusSubscription = "rc.ca.webapi.main.sub";
    public const string EventBusTopic = "rc.ca.webapi.topic";

    public static readonly Dictionary<string,string> MembersImportFields = new Dictionary<string,string>() { { "Name", "Name" }, { "Gender", "Gender" }, { "Qualification", "Qualification" } };
}
