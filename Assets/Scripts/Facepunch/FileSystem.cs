using System.Linq;
using UnityEngine;

public static class FileSystem
{
    public static bool LogDebug;

    public static bool LogTime;

    private static string[] _assetList;

    public static FileSystemBackend Backend;

    public static GameObject[] LoadPrefabs(string folder)
    {
        return Backend.LoadPrefabs(folder);
    }

    public static GameObject LoadPrefab(string filePath)
    {
        return Backend.LoadPrefab(filePath);
    }

    public static string[] FindAll(string folder, string search = "")
    {
        return Backend.FindAll(folder, search);
    }

    public static T[] LoadAll<T>(string folder, string search = "") where T : UnityEngine.Object
    {
        folder = folder.ToLower();
        return Backend.LoadAll<T>(folder, search);
    }

    public static T Load<T>(string filePath, bool complain = true) where T : UnityEngine.Object
    {
        filePath = filePath.ToLower();
        T t = Backend?.Load<T>(filePath);
        if (complain && t == null)
        {
            UnityEngine.Debug.LogWarning(string.Concat(new object[]
            {
                "[FileSystem] Not Found: ",
                filePath,
                " (",
                typeof(T),
                ")"
            }));
        }
        return t;
    }

    public static string[] GetAssetList()
    {
        if (_assetList == null)
        {
            _assetList = (
                from x in GameManifest.Current.prefabProperties
                select x.name into x
                select x).ToArray<string>();
        }

        return _assetList;
    }
}
