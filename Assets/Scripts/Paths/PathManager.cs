using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

[DisallowMultipleComponent]
public class PathManager : MonoBehaviour, IManagerEx
{
    protected static Transform Parent { get; private set; }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void Restore()
    {
        FindOrCreateParent();
        RestoreSerializedData();
    }

    private static void FindOrCreateParent()
    {
        if (Parent != null)
            return;

        Parent = GameObject.FindWithTag("Paths")?.transform;
        if (Parent == null)
        {
            GameObject parent = new GameObject("Paths");
            parent.tag = "Paths";
            parent.AddComponent<PositionLock>();
            Parent = parent.transform;
        }
    }

    private static void RestoreSerializedData()
    {
        string path = Path.Combine(Application.dataPath, "paths.bytes");
        if (File.Exists(path))
        {
            UnityEngine.Debug.Log("Attempting to restore path data..." +
                "");
            List<ProtoBuf.PathData> pathList = Serialization.Deserialize<List<ProtoBuf.PathData>>(File.ReadAllBytes(path));
            if (pathList != null)
            {
                Transform[] children = Parent.GetDirectChildren();
                for (int i = 0; i < pathList.Count; i++)
                {
                    ProtoBuf.PathData pathData = pathList[i];

                    for (int y = 0; y < children.Length; y++)
                    {
                        Transform child = children[y];
                        if (child.gameObject.name == pathData?.name)
                        {
                            child.GetComponent<PathData>().serializedData = pathData;
                            break;
                        }
                    }
                }
            }

            File.Delete(path);
            UnityEngine.Debug.Log("Restored Path Data");
        }
    }

    private void OnEnable()
    {
        FindOrCreateParent();
    }

    public void Cleanup()
    {
        PathData[] pathData = UnityEngine.Object.FindObjectsOfType<PathData>();
        if (pathData?.Length > 0)
        {
            for (int i = pathData.Length - 1; i >= 0; i--)
                DestroyImmediate(pathData[i]?.gameObject);
        }
    }

    public void Save(ref WorldSerialization blob)
    {
        List<ProtoBuf.PathData> pathList = new List<ProtoBuf.PathData>();

        PathData[] pathData = UnityEngine.Object.FindObjectsOfType<PathData>();
        if (pathData?.Length > 0)
        {
            for (int i = 0; i < pathData.Length; i++)
            {
                PathData data = pathData[i];
                if (data == null)
                    continue;
                
                pathList.Add(data.GetPathData());
            }
        }

        blob.world.paths = pathList;
    }

    public IEnumerator Load(World.Data world)
    {
        ActionProgressBar.UpdateProgress("Loading Map Prefabs", 0f);
        yield return null;
        yield return null;

        Stopwatch sw = Stopwatch.StartNew();

        if (Parent == null)
            FindOrCreateParent();

        for (int i = 0; i < world.pathData.Count; i++)
        {
            if (sw.Elapsed.TotalSeconds > 1f || i == 0 || i == world.pathData.Count - 1)
            {
                ActionProgressBar.UpdateProgress("Loading Map Paths", (float)world.pathData.Count / (float)i);
                yield return null;
                yield return null;
                sw.Reset();
            }

            ProtoBuf.PathData serializedData = world.pathData[i];
            
            PathData pathData = new GameObject(serializedData.name).AddComponent<PathData>();

            pathData.transform.SetParent(Parent);

            pathData.Set(serializedData);            
        }

        sw.Stop();
    }

    public static void CreatePath(PathType pathType, Vector3 position)
    {
        if (Parent == null)
            FindOrCreateParent();

        ProtoBuf.PathData serializedData = CreateEmptyPathData(pathType);
        PathData pathData = new GameObject(serializedData.name).AddComponent<PathData>();

        pathData.transform.SetParent(Parent);

        pathData.Set(serializedData);

        pathData.AddNode(position + (Vector3.forward * 2));
        pathData.AddNode(position + -(Vector3.forward * 2));        
    }

    internal static ProtoBuf.PathData CreateEmptyPathData(PathType pathType)
    {
        switch (pathType)
        {
            case PathType.Road:
                return new ProtoBuf.PathData
                {
                    end = true,
                    innerFade = 1,
                    innerPadding = 1,
                    meshOffset = 0,
                    name = "Road ",
                    nodes = new ProtoBuf.VectorData[0],
                    outerFade = 8,
                    outerPadding = 1,
                    randomScale = 0.75f,
                    splat = 128,
                    spline = false,
                    start = true,
                    terrainOffset = -0.5f,
                    topology = 2048,
                    width = 10
                };
            case PathType.River:
                return new ProtoBuf.PathData
                {
                    end = true,
                    innerFade = 8,
                    innerPadding = 0.5f,
                    meshOffset = -0.4f,
                    name = "River ",
                    nodes = new ProtoBuf.VectorData[0],
                    outerFade = 16,
                    outerPadding = 0.5f,
                    randomScale = 0.75f,
                    splat = 64,
                    spline = false,
                    start = true,
                    terrainOffset = -2,
                    topology = 16384,
                    width = 24
                };
            case PathType.Powerline:
                return new ProtoBuf.PathData
                {
                    end = true,
                    innerFade = 0,
                    innerPadding = 0,
                    meshOffset = 0,
                    name = "Powerline ",
                    nodes = new ProtoBuf.VectorData[0],
                    outerFade = 0,
                    outerPadding = 0,
                    randomScale = 0,
                    splat = 0,
                    spline = false,
                    start = true,
                    terrainOffset = 0,
                    topology = 0,
                    width = 0
                };
        }
        return null;
    }
}

public enum PathType { Road, River, Powerline }
