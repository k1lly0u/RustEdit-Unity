using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AssetBundleBackend : FileSystemBackend
{
    private AssetBundle rootBundle;

    private AssetBundleManifest manifest;

    public static Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>(StringComparer.OrdinalIgnoreCase);

    public static Dictionary<string, AssetBundle> files = new Dictionary<string, AssetBundle>(StringComparer.OrdinalIgnoreCase);

    private string assetPath;

    public AssetBundleBackend(string assetRoot, AssetBundle bundle = null)
    {
        isError = false;
        assetPath = Path.GetDirectoryName(assetRoot) + Path.DirectorySeparatorChar;
        rootBundle = bundle == null ? AssetBundle.LoadFromFile(assetRoot) : bundle;
        if (rootBundle == null)
        {
            LoadError("Couldn't load root AssetBundle - " + assetRoot);
            return;
        }

        AssetBundleManifest[] array = rootBundle.LoadAllAssets<AssetBundleManifest>();
        if (array.Length != 1)
        {
            LoadError("Couldn't find AssetBundleManifest - " + array.Length);
            return;
        }
        manifest = array[0];

        foreach (string bundleName in manifest.GetAllAssetBundles())
        {
            LoadBundle(bundleName);
            if (isError)
            {
                return;
            }
        }

        BuildFileIndex();
    }

    private void LoadBundle(string bundleName)
    {
        if (bundles.ContainsKey(bundleName))
        {
            return;
        }
        string text = assetPath + bundleName;
        AssetBundle assetBundle = AssetBundle.LoadFromFile(text);
        if (assetBundle == null)
        {
            LoadError("Couldn't load AssetBundle - " + text);
            return;
        }
        bundles.Add(bundleName, assetBundle);
    }

    private void BuildFileIndex()
    {
        files.Clear();
        foreach (KeyValuePair<string, AssetBundle> keyValuePair in bundles)
        {
            if (!keyValuePair.Key.StartsWith("content", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (string key in keyValuePair.Value.GetAllAssetNames())
                {
                    files.Add(key, keyValuePair.Value);
                }
            }
        }
    }

    public void UnloadBundles()
    {
        manifest = null;
        foreach (KeyValuePair<string, AssetBundle> keyValuePair in bundles)
        {
            keyValuePair.Value.Unload(false);
            UnityEngine.Object.DestroyImmediate(keyValuePair.Value);
        }
        bundles.Clear();
        if (rootBundle)
        {
            rootBundle.Unload(false);
            UnityEngine.Object.DestroyImmediate(rootBundle);
            rootBundle = null;
        }
    }

    protected override T LoadAsset<T>(string filePath)
    {
        AssetBundle assetBundle = null;
        if (!files.TryGetValue(filePath, out assetBundle))
        {
            return (T)((object)null);
        }
        return assetBundle.LoadAsset<T>(filePath);
    }

    protected override string[] LoadAssetList(string folder, string search)
    {
        List<string> list = new List<string>();
        foreach (KeyValuePair<string, AssetBundle> keyValuePair in from x in files
                                                                   where x.Key.StartsWith(folder, StringComparison.InvariantCultureIgnoreCase)
                                                                   select x)
        {
            if (string.IsNullOrEmpty(search) || keyValuePair.Key.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                list.Add(keyValuePair.Key);
            }
        }
        return list.ToArray();
    }
}
