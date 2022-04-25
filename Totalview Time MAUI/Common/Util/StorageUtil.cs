using Newtonsoft.Json;

namespace Totalview_Time_MAUI.Common.Util;

internal static class StorageUtil
{
    public const string StorageKey = nameof(StorageKey);

    public static T DeserializeObjec<T>(string serializedString)
    {
        return JsonConvert.DeserializeObject<T>(serializedString);
    }

    public static string SerializeObject<T>(T unserializedObject)
    {
        return JsonConvert.SerializeObject(unserializedObject);
    }
}
