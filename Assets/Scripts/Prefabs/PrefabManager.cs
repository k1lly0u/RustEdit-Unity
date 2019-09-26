using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[DisallowMultipleComponent]
public class PrefabManager : MonoBehaviour, IManagerEx
{        
    protected static Transform Parent { get; private set; }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void Restore()
    {
        FindOrCreateParent();
    }

    private static void FindOrCreateParent()
    {
        if (Parent != null)
            return;

        Parent = GameObject.FindWithTag("Prefabs")?.transform;
        if (Parent == null)
        {
            GameObject parent = new GameObject("Prefabs");
            parent.tag = "Prefabs";
            parent.AddComponent<PositionLock>();
            Parent = parent.transform;
        }
    }

    private void OnEnable()
    {
        FindOrCreateParent();
    }

    public void Cleanup()
    {
        PrefabData[] prefabData = UnityEngine.Object.FindObjectsOfType<PrefabData>();
        if (prefabData?.Length > 0)
        {
            for (int i = prefabData.Length - 1; i >= 0; i--)
                DestroyImmediate(prefabData[i]?.gameObject);
        }
    }

    public void Save(ref WorldSerialization blob)
    {
        ActionProgressBar.UpdateProgress("Saving Prefabs", 0f);

        List<ProtoBuf.PrefabData> prefabList = new List<ProtoBuf.PrefabData>();

        PrefabData[] prefabData = UnityEngine.Object.FindObjectsOfType<PrefabData>();
        if (prefabData?.Length > 0)
        {
            for (int i = 0; i < prefabData.Length; i++)
            {
                PrefabData data = prefabData[i];

                if (prefabData == null || data.gameObject == null)
                    continue;

                prefabList.Add(data.GetPrefabData());
            }
        }

        blob.world.prefabs = prefabList;
    }

    public IEnumerator Load(World.Data world)
    {
        ActionProgressBar.UpdateProgress("Loading Map Prefabs", 0f);
        yield return null;
        yield return null;

        Stopwatch sw = Stopwatch.StartNew();        

        for (int i = 0; i < world.prefabData.Count; i++)
        {
            if (sw.Elapsed.TotalSeconds > 1f || i == 0 || i == world.prefabData.Count - 1)
            {
                ActionProgressBar.UpdateProgress("Loading Map Prefabs", (float)world.prefabData.Count / (float)i);
                yield return null;
                yield return null;
                sw.Reset();
            }

            ProtoBuf.PrefabData prefabData = world.prefabData[i];

            CreatePrefab(StringPool.Get(prefabData.id), prefabData.category, prefabData.position, prefabData.rotation, prefabData.scale);
        }

        sw.Stop();
    }

    public static GameObject CreatePrefab(string strPrefab, string category, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        if (Parent == null)
            FindOrCreateParent();

        GameObject gameObject = Instantiate(strPrefab, pos, rot);
        if (gameObject)
        {
            gameObject.transform.localScale = scale;

            gameObject.SetLayerRecursive(9);

            gameObject.SetActive(true);

            PrefabData prefabData = gameObject.AddComponent<PrefabData>();
            
            prefabData.category = category;

            gameObject.transform.SetParent(Parent);
        }
        return gameObject;
    }

    private static GameObject Instantiate(string strPrefab, Vector3 pos, Quaternion rot)
    {
        strPrefab = strPrefab.ToLower();

        GameObject gameObject = FindPrefab(strPrefab);
        if (!gameObject)
        {
            UnityEngine.Debug.LogError(string.Concat("Couldn't find prefab \"", strPrefab, "\""));
            return null;
        }

        GameObject newObject = UnityEngine.Object.Instantiate(gameObject, pos, rot);
        newObject.name = strPrefab;
        newObject.transform.localScale = gameObject.transform.localScale;
        return newObject;
    }

    private static GameObject FindPrefab(string strPrefab)
    {
        GameObject gameObject = FileSystem.LoadPrefab(strPrefab);
        if (gameObject == null)
        {
            return null;
        }
        return gameObject;
    }
}
