using System.Text;

namespace RC.CA.SharedKernel.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Convert string to UTF8 byte array
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] ToByteArrayExt(this String str)
    {
        if (string.IsNullOrEmpty(str))
            return new byte[0];
        return Encoding.UTF8.GetBytes(str);
    }
    /// <summary>
    /// Escape html
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string EscapeHtmlExt(this String str)
    {
        if (string.IsNullOrEmpty(str))
            return "";
        return System.Web.HttpUtility.HtmlEncode(str);
    }
    /// <summary>
    /// Return null if whitespace or null, useful for null coalesce check.
    /// Example: item.PhotoUrl.NullIfWhiteSpaceExt() ?? "noimage.jpg"
    /// </summary>
    /// <param name="str"></param>
    /// <returns>Null if white space</returns>
    public static string NullIfWhiteSpaceExt(this String str)
    {
        return string.IsNullOrEmpty(str) ? null : str;
    }
    /// <summary>
    /// Convert string to decimal
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <param name="defaultValue">Default value</param>
    /// <returns></returns>
    public static decimal ToDecimalExt(this String str, decimal defaultValue)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;
        try
        {
            return Convert.ToDecimal(str);
        }
        catch
        {
            return defaultValue;
        }
    }
    /// <summary>
    /// To integer extension
    /// </summary>
    /// <param name="str"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal ToIntExt(this String str, int defaultValue = 0)
    {
        if (string.IsNullOrEmpty(str))
            return defaultValue;

        if (int.TryParse(str, out int returnVal))
            return returnVal;
        else
            return defaultValue;
    }
    /// <summary>
    /// Get element from array Separator 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="Index"></param>
    /// <param name="Sepchar"></param>
    /// <returns></returns>
    public static string GetArrayElementExt(this String str, int Index, char Sepchar = '_')
    {
        if (string.IsNullOrEmpty(str))
            return "";
        try
        {
            var values = str.Split(new char[] { Sepchar });
            if (values.Length >= Index)
                return values[1];
        }
        catch { }
        return "";
    }

    /// <summary>
    /// Mask sensitive data in josn, connection strings
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MaskSensitiveDataExt(this String str)
    {
        System.Collections.Generic.Dictionary<string, string> maskValue = new Dictionary<string, string>
                {
                    { "JsonPassword","\"password\".+?\".+?\"" },
                    { "SqlPassword", "password=.*?;" },
                    { "SqlPwd", "pwd=.*?;" }
                };
        foreach (var maskItem in maskValue)
            str = System.Text.RegularExpressions.Regex.Replace(str, maskItem.Value, $"{maskItem.Key}(*Redacted)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return str;
    }
    /// <summary>
    /// Convert email into System.Net.Mail.MailAddress 
    /// </summary>
    /// <param name="EmailAsString"></param>
    /// <returns></returns>
    public static System.Net.Mail.MailAddress CvtToMailAddressExt(this String str)
    {
        System.Net.Mail.MailAddress email = null;
        try
        {
            email = new System.Net.Mail.MailAddress(str);
        }
        catch (Exception) { email = null; }
        return email;
    }
    /// <summary>
    /// Remove all non alphanumeric characters from string 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string RemoveAllAlphaNumExt(this String s)
    {
        return System.Text.RegularExpressions.Regex.Replace(s, "[^a-zA-Z0-9]", "");
    }
    /// <summary>
    /// Remove all non alphanumeric characters from string leave _. Used for file names
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string SafeFileNameExt(this String s)
    {
        return System.Text.RegularExpressions.Regex.Replace(s, @"[^a-zA-Z0-9\\_]", "");
    }
    /// <summary>
    /// Check if string is valid email
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool IsValidEmailExt(this String s)
    {
        bool emailOK = true;
        try
        {
            System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(s);
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, "[\\w\\.-<-_']+(\\+[\\w-]*)?@([\\w-']+\\.)+[\\w->]+", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                emailOK = false;
        }
        catch { emailOK = false; }
        return emailOK;
    }
}
