using Newtonsoft.Json;
using Totalview_Time_Smartclient.MVVM.Model.Services.Authentication;

namespace Totalview_Time_Smartclient.MVVM.Model.Util;

internal static class StorageUtil
{
    // Values
    public static StorageValues StorageValues;

    // Keys
    public static StorageKeys StorageKeys;

    static StorageUtil()
    {
        StorageValues = new StorageValues(null, null);
        StorageKeys = new StorageKeys();
    }

    public static T DeserializeObjec<T>(string serializedString)
    {
        return JsonConvert.DeserializeObject<T>(serializedString);
    }

    public static string SerializeObject<T>(T unserializedObject)
    {
        return JsonConvert.SerializeObject(unserializedObject);
    }
}

internal record StorageValues
{
    // Values
    public readonly AuthenticationCredentials AuthenticationCredentialsValue;
    public readonly OpenIdOptions OpenIdOptionsValue;

    public StorageValues(AuthenticationCredentials authenticationCredentialsValue, OpenIdOptions openIdOptionsValue)
    {
        AuthenticationCredentialsValue = authenticationCredentialsValue;
        OpenIdOptionsValue = openIdOptionsValue;
    }
}

internal record StorageKeys
{
    // Keys
    public readonly string AuthenticationCredentialsKey;
    public readonly string OpenIdOptionsKey;
    public readonly string RegisterVersionOptionsKey;

    public StorageKeys()
    {
        AuthenticationCredentialsKey = nameof(AuthenticationCredentialsKey);
        OpenIdOptionsKey = nameof(OpenIdOptionsKey);
        RegisterVersionOptionsKey = nameof(RegisterVersionOptionsKey);
    }
}
