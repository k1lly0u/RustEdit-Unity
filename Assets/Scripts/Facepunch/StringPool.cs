using System;
using System.Collections.Generic;
using UnityEngine;

public class StringPool
{
    public static Dictionary<uint, string> toString;

    public static Dictionary<string, uint> toNumber;

    private static bool initialized;

    public static uint closest;

    static StringPool()
    {
    }

    public StringPool()
    {
    }

    public static uint Add(string str)
    {
        uint num = 0;
        if (!toNumber.TryGetValue(str, out num))
        {
            num = str.ManifestHash();
            toString.Add(num, str);
            toNumber.Add(str, num);
        }
        return num;
    }

    public static string Get(uint i)
    {
        string str;
        if (i == 0)
        {
            return string.Empty;
        }
        Init();
        if (toString.TryGetValue(i, out str))
        {
            return str;
        }
        Debug.LogWarning(string.Concat("GetString - no string for ID", i));
        return string.Empty;
    }

    public static uint Get(string str)
    {
        uint num;
        if (string.IsNullOrEmpty(str))
        {
            return (uint)0;
        }
        Init();
        if (toNumber.TryGetValue(str, out num))
        {
            return num;
        }
        Debug.LogWarning(string.Concat("GetNumber - no number for string ", str));
        return (uint)0;
    }

    public static bool Exists(string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;

        Init();
        return toNumber.ContainsKey(str);
    }

    private static void Init()
    {
        if (initialized)
        {
            return;
        }
        toString = new Dictionary<uint, string>();
        toNumber = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);
        GameManifest gameManifest = FileSystem.Load<GameManifest>("Assets/manifest.asset", true);
        for (uint i = 0; i < gameManifest.pooledStrings.Length; i++)
        {
            toString.Add(gameManifest.pooledStrings[i].hash, gameManifest.pooledStrings[i].str);
            toNumber.Add(gameManifest.pooledStrings[i].str, gameManifest.pooledStrings[i].hash);
        }
        initialized = true;
        closest = Get("closest");

    }
}