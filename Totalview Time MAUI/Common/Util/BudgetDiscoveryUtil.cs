using System.Text.RegularExpressions;

namespace Totalview_Time_Smartclient.Common.Util;

internal static class BudgetDiscoveryUtil
{
    public static string formatServerAddress(string serverAddress)
    {
        if (string.IsNullOrEmpty(serverAddress))
        {
            return null;
        }
        return StartHttpsRegexMatch(serverAddress) + serverAddress + ":9000/TVWCF";
    }

    public static string formatAuthorityAddress(string authorityAddress)
    {
        return StartHttpsRegexMatch(authorityAddress) + authorityAddress + "/TotalviewAuthentication";
    }

    public static string StartHttpsRegexMatch(string address)
    {
        if (!Regex.IsMatch(address, @"^https?:\/\/", RegexOptions.IgnoreCase))
        {
            address = "https://" + address;
        }
        return address;
    }
}
