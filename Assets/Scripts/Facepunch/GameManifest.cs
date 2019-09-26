using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameManifest : ScriptableObject
{    
    public static GameManifest Current
    {
        get
        {
            if (GameManifest.loadedManifest != null)
            {
                return GameManifest.loadedManifest;
            }
            GameManifest.Load();
            return GameManifest.loadedManifest;
        }
    }

    public static void Load()
    {
        if (GameManifest.loadedManifest != null)
        {
            return;
        }

        Debug.Log("Loading Game Manifest...");

        GameManifest.loadedManifest = FileSystem.Load<GameManifest>("Assets/manifest.asset");
        foreach (GameManifest.PrefabProperties prefabProperties in GameManifest.loadedManifest.prefabProperties)
        {
            GameManifest.guidToPath.Add(prefabProperties.guid, prefabProperties.name);
            GameManifest.pathToGuid.Add(prefabProperties.name, prefabProperties.guid);
        }
        foreach (GameManifest.GuidPath guidPath in GameManifest.loadedManifest.guidPaths)
        {
            if (!GameManifest.guidToPath.ContainsKey(guidPath.guid))
            {
                GameManifest.guidToPath.Add(guidPath.guid, guidPath.name);
                GameManifest.pathToGuid.Add(guidPath.name, guidPath.guid);
            }
        }
        Debug.Log(GameManifest.GetMetadataStatus());
    }

    internal static Dictionary<string, string[]> LoadEffectDictionary()
    {
        GameManifest.EffectCategory[] array = GameManifest.loadedManifest.effectCategories;
        Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
        foreach (GameManifest.EffectCategory effectCategory in array)
        {
            dictionary.Add(effectCategory.folder, effectCategory.prefabs.ToArray());
        }
        return dictionary;
    }

    internal static string GUIDToPath(string guid)
    {
        if (string.IsNullOrEmpty(guid))
        {
            Debug.Log("GUIDToPath: guid is empty");
            return string.Empty;
        }
        GameManifest.Load();
        string result;
        if (GameManifest.guidToPath.TryGetValue(guid, out result))
        {
            return result;
        }
        Debug.Log("GUIDToPath: no path found for guid " + guid);
        return string.Empty;
    }

    internal static UnityEngine.Object GUIDToObject(string guid)
    {
        UnityEngine.Object result = null;
        if (GameManifest.guidToObject.TryGetValue(guid, out result))
        {
            return result;
        }
        string text = GameManifest.GUIDToPath(guid);
        if (string.IsNullOrEmpty(text))
        {
            Debug.Log("Missing file for guid " + guid);
            GameManifest.guidToObject.Add(guid, null);
            return null;
        }
        UnityEngine.Object @object = FileSystem.Load<UnityEngine.Object>(text, true);
        GameManifest.guidToObject.Add(guid, @object);
        return @object;
    }

    internal static void Invalidate(string path)
    {
        string key;
        if (!GameManifest.pathToGuid.TryGetValue(path, out key))
        {
            return;
        }
        UnityEngine.Object @object;
        if (!GameManifest.guidToObject.TryGetValue(key, out @object))
        {
            return;
        }
        if (@object != null)
        {
            UnityEngine.Object.DestroyImmediate(@object, true);
        }
        GameManifest.guidToObject.Remove(key);
    }

    private static string GetMetadataStatus()
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (GameManifest.loadedManifest != null)
        {
            stringBuilder.Append("Manifest Metadata Loaded");
            stringBuilder.AppendLine();
            stringBuilder.Append("\t");
            stringBuilder.Append(GameManifest.loadedManifest.pooledStrings.Length.ToString());
            stringBuilder.Append(" pooled strings");
            stringBuilder.AppendLine();
            stringBuilder.Append("\t");
            stringBuilder.Append(GameManifest.loadedManifest.meshColliders.Length.ToString());
            stringBuilder.Append(" mesh colliders");
            stringBuilder.AppendLine();
            stringBuilder.Append("\t");
            stringBuilder.Append(GameManifest.loadedManifest.prefabProperties.Length.ToString());
            stringBuilder.Append(" prefab properties");
            stringBuilder.AppendLine();
            stringBuilder.Append("\t");
            stringBuilder.Append(GameManifest.loadedManifest.effectCategories.Length.ToString());
            stringBuilder.Append(" effect categories");
            stringBuilder.AppendLine();
            stringBuilder.Append("\t");
            stringBuilder.Append(GameManifest.loadedManifest.entities.Length.ToString());
            stringBuilder.Append(" entity names");
            stringBuilder.AppendLine();
            stringBuilder.Append("\t");
            stringBuilder.Append(GameManifest.loadedManifest.skinnables.Length.ToString());
            stringBuilder.Append(" skinnable names");
        }
        else
        {
            stringBuilder.Append("Manifest Metadata Missing");
        }
        return stringBuilder.ToString();
    }

    private static string GetAssetStatus()
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (GameManifest.loadedManifest != null)
        {
            stringBuilder.Append("Manifest Assets Loaded");
            stringBuilder.AppendLine();
        }
        else
        {
            stringBuilder.Append("Manifest Assets Missing");
        }
        return stringBuilder.ToString();
    }

    public GameManifest.PooledString[] pooledStrings;

    public GameManifest.MeshColliderInfo[] meshColliders;

    public GameManifest.PrefabProperties[] prefabProperties;

    public GameManifest.EffectCategory[] effectCategories;

    public GameManifest.GuidPath[] guidPaths;

    public string[] skinnables;

    public string[] entities;

    internal static GameManifest loadedManifest;

    internal static Dictionary<string, string> guidToPath = new Dictionary<string, string>();

    internal static Dictionary<string, string> pathToGuid = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    internal static Dictionary<string, UnityEngine.Object> guidToObject = new Dictionary<string, UnityEngine.Object>();

    [Serializable]
    public struct PooledString
    {
        [HideInInspector]
        public string str;

        public uint hash;
    }

    [Serializable]
    public class MeshColliderInfo
    {
        [HideInInspector]
        public string name;

        public uint hash;

        public PhysicMaterial physicMaterial;
    }

    [Serializable]
    public class PrefabProperties
    {
        [HideInInspector]
        public string name;

        public string guid;

        public uint hash;

        public bool pool;
    }

    [Serializable]
    public class EffectCategory
    {
        [HideInInspector]
        public string folder;

        public List<string> prefabs;
    }

    [Serializable]
    public class GuidPath
    {
        [HideInInspector]
        public string name;

        public string guid;
    }
}
