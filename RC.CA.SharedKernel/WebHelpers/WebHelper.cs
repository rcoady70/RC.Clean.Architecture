using Microsoft.AspNetCore.Http;

namespace RC.CA.SharedKernel.WebHelpers;

public class WebHelperRequests
{
    public static string GetHeaderValue(HttpContext httpContext,string key,string defaultValue ="")
    {
        //Try to get requestId from header. Used to tie error messages between mvc site and api site
        if (!httpContext.Request.Headers.TryGetValue(key, out var value))
            value = defaultValue;
        return value;
    }
    /// <summary>
    /// Get ip address helper
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetIpAddress(HttpContext httpContext)
    {
        if (httpContext == null)
            return "No-IP-Found";
        string? ip = null;
        try
        {
            //Get ip from header first then drop through the different options
            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                ip = httpContext.Request.Headers["X-Forwarded-For"].ToString();
            else
            {
                // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
                if (string.IsNullOrEmpty(ip) && httpContext?.Connection?.RemoteIpAddress != null)
                    ip = httpContext.Connection.RemoteIpAddress.ToString();
                else
                {
                    if (string.IsNullOrEmpty(ip))
                    {
                        if (httpContext.Request.Headers.ContainsKey("REMOTE_ADDR"))
                            ip = httpContext.Request.Headers["REMOTE_ADDR"].ToString();
                        else
                            ip = "No-IP-Found";
                    }
                }
            }
        }
        catch
        { }
        return ip;
    }
}
