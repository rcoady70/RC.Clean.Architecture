using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.ValueTypes;

public  class EmailAddress : ValueObject
{
    private static readonly Regex Rfc2822Regex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.ECMAScript);
    public string Address { get; private set; } = "";
    public string? DisplayName { get; private set; } = "";
    /// <summary>
    /// Construct email address wither pass email as test@gmail.com or with display name "Richard Smith" <test@gmail.com>
    /// </summary>
    /// <param name="address"></param>
    public EmailAddress(string address)
    {
        //Richard Smith <rsmith@gmail.com>
        var addressParts = address.Trim().Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
        if (addressParts.Length > 1)
        {
            DisplayName = addressParts[0].Replace("\"","");
            Address = addressParts[1].Replace("<", "").Replace(">", "");
        }
        else
        {
            if (addressParts.Length == 1)
                DisplayName = addressParts[0].Replace("<", "").Replace(">", ""); ;
        }
    }
    [JsonConstructor]
    public EmailAddress(string address, string displayName)
    {
        Address = address;
        DisplayName = displayName;
    }
    /// <summary>
    /// Check if email address is valid. Only address part is checked
    /// </summary>
    /// <returns></returns>
    public bool IsValidAddress()
    {
        if (string.IsNullOrEmpty(Address))
            return false;
        return Rfc2822Regex.IsMatch(Address);
    }

    /// <summary>
    /// Get value to check equality
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return $"{DisplayName} <{Address}>";
    }
    /// <summary>
    /// Stringify email address
    /// </summary>
    /// <returns></returns>
    protected new string ToString()
    {
        if(string.IsNullOrEmpty(DisplayName))
            return $"{Address}";
        else
            return $"\"{DisplayName}\" <{Address}>";
    }
}
