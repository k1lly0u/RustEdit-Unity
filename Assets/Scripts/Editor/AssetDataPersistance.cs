using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

class AssetDataPersistance : AssetPostprocessor
{
    private void OnPreprocessAsset()
    {
        Selection.objects = new Object[0];

        WriteTextureMaps();

        WritePathData();

        UnloadAssetBundles();

        ActionProgressBar.Close();
    }

    private void WriteTextureMaps()
    {
        if (TerrainManager.Splat != null)
            File.WriteAllBytes(Path.Combine(Application.dataPath, "splat.bytes"), TerrainManager.Splat.GetBytes());

        if (TerrainManager.Biome != null)
            File.WriteAllBytes(Path.Combine(Application.dataPath, "biome.bytes"), TerrainManager.Biome.GetBytes());

        if (TerrainManager.Alpha != null)
            File.WriteAllBytes(Path.Combine(Application.dataPath, "alpha.bytes"), TerrainManager.Alpha.GetBytes());

        if (TerrainManager.Topology != null)
            File.WriteAllBytes(Path.Combine(Application.dataPath, "topology.bytes"), TerrainManager.Topology.GetBytes());

        if (TerrainManager.Water != null)
            File.WriteAllBytes(Path.Combine(Application.dataPath, "water.bytes"), TerrainManager.Water.GetBytes());
    }

    private void WritePathData()
    {
        PathData[] pathData = UnityEngine.Object.FindObjectsOfType<PathData>();
        if (pathData?.Length > 0)
        {
            List<ProtoBuf.PathData> pathList = new List<ProtoBuf.PathData>();

            for (int i = 0; i < pathData.Length; i++)
            {
                PathData data = pathData[i];
                if (data == null)
                    continue;
                                
                pathList.Add(data.GetPathData());
            }

            byte[] bytes = Serialization.Serialize<List<ProtoBuf.PathData>>(pathList);
            if (bytes != null)
                File.WriteAllBytes(Path.Combine(Application.dataPath, "paths.bytes"), bytes);
        }
    }

    private void UnloadAssetBundles()
    {
        if (FileSystem.Backend != null)
            (FileSystem.Backend as AssetBundleBackend).UnloadBundles();
    }
}
