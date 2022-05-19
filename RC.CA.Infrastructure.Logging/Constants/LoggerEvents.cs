using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RC.CA.Infrastructure.Logging.Constants;
/// <summary>
/// Logger events uses to separate log events for analysis
/// </summary>
public static class LoggerEvents
{
    public static EventId ErrorEvt = new EventId(999000, "Error");
    public static EventId SecurityEvtCSP = new EventId(999100, "SecurityCSP");
    public static EventId SecurityEvtExpiry = new EventId(999101, "SecurityExpireCookie");
    public static EventId DatabaseEvt = new EventId(999200, "Database");
    public static EventId CriticalDbEvt = new EventId(999201, "CriticalDatabase");
    public static EventId CriticalEvt = new EventId(999300, "Critical");
    public static EventId Warning404Evt = new EventId(999400, "Warning");
    public static EventId PerformanceEvt = new EventId(999500, "Performance");
    public static EventId APIEvt = new EventId(999600, "APIFailure");
    public static EventId InfoEvt = new EventId(999700, "Information");
    public static EventId DebugLifeCycle = new EventId(999800, "DebugLifeCycle");
}
