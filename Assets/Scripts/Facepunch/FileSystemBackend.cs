using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FileSystemBackend
{
    protected abstract T LoadAsset<T>(string filePath) where T : UnityEngine.Object;

    protected abstract string[] LoadAssetList(string folder, string search);

    public bool isError;

    public string loadingError = string.Empty;

    public static Dictionary<string, UnityEngine.Object> cache = new Dictionary<string, UnityEngine.Object>();

    public GameObject[] LoadPrefabs(string folder)
    {
        if (!folder.EndsWith("/", StringComparison.CurrentCultureIgnoreCase))
        {
            Debug.LogWarning("LoadPrefabs - folder should end in '/' - " + folder);
        }
        if (!folder.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
        {
            Debug.LogWarning("LoadPrefabs - should start with assets/ - " + folder);
        }
        return LoadAll<GameObject>(folder, ".prefab");
    }

    public GameObject LoadPrefab(string filePath)
    {
        if (!filePath.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
        {
            Debug.LogWarning("LoadPrefab - should start with assets/ - " + filePath);
        }
        return Load<GameObject>(filePath);
    }

    public string[] FindAll(string folder, string search = "")
    {
        return LoadAssetList(folder, search);
    }

    public T[] LoadAll<T>(string folder, string search = "") where T : UnityEngine.Object
    {
        List<T> list = new List<T>();
        foreach (string filePath in FindAll(folder, search))
        {
            T t = Load<T>(filePath);
            if (t != null)
            {
                list.Add(t);
            }
        }
        return list.ToArray();
    }

    public T Load<T>(string filePath) where T : UnityEngine.Object
    {
        T t = (T)((object)null);
        if (cache.ContainsKey(filePath))
        {
            t = (cache[filePath] as T);
        }
        else
        {
            t = LoadAsset<T>(filePath);
            if (t != null)
            {
                cache.Add(filePath, t);
            }
        }
        return t;
    }

    protected void LoadError(string err)
    {
        Debug.LogError(err);
        loadingError = err;
        isError = true;
    }
}
