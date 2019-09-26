using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;

[DisallowMultipleComponent]
public class TerrainManager : MonoBehaviour, IManagerEx
{
    public static SplatMap Splat { get; private set; }
    public static AlphaMap Alpha { get; private set; }
    public static BiomeMap Biome { get; private set; }
    public static TopologyMap Topology { get; private set; }
    public static WaterMap Water { get; private set; }

    public static Terrain Terrain { get; private set; }

    public static GameObject WaterPlane { get; private set; }

    public PaintType PaintMode { get; private set; } = PaintType.Splat;

    public TerrainTopology.Enum SelectedTopology { get; private set; } = TerrainTopology.Enum.Field;

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void Restore()
    {
        if (!HasValidTerrain())
            return;

        Debug.Log("Script reload detected. Attempting to restore texture data...");

        string path = Path.Combine(Application.dataPath, "splat.bytes");
        if (File.Exists(path))
        {
            TerrainMap<byte> byteMap = new TerrainMap<byte>(File.ReadAllBytes(path), 8);
            Splat = new SplatMap(byteMap);
            File.Delete(path);
            Debug.Log("Restored Splat Maps");
        }

        path = Path.Combine(Application.dataPath, "biome.bytes");
        if (File.Exists(path))
        {
            TerrainMap<byte> byteMap = new TerrainMap<byte>(File.ReadAllBytes(path), 4);
            Biome = new BiomeMap(byteMap);
            File.Delete(path);
            Debug.Log("Restored Biome Maps");
        }

        path = Path.Combine(Application.dataPath, "alpha.bytes");
        if (File.Exists(path))
        {
            TerrainMap<byte> byteMap = new TerrainMap<byte>(File.ReadAllBytes(path), 1);
            Alpha = new AlphaMap(byteMap);
            File.Delete(path);
            Debug.Log("Restored Alpha Maps");
        }

        path = Path.Combine(Application.dataPath, "topology.bytes");
        if (File.Exists(path))
        {
            TerrainMap<int> byteMap = new TerrainMap<int>(File.ReadAllBytes(path), 1);
            Topology = new TopologyMap(byteMap);
            File.Delete(path);
            Debug.Log("Restored Topology Maps");
        }

        path = Path.Combine(Application.dataPath, "water.bytes");
        if (File.Exists(path))
        {
            TerrainMap<short> byteMap = new TerrainMap<short>(File.ReadAllBytes(path), 1);
            Water = new WaterMap(byteMap);
            File.Delete(path);
            Debug.Log("Restored Water Maps");
        }
    }

    public void Cleanup()
    {
        if (Terrain != null)
            UnityEngine.Object.DestroyImmediate(Terrain.gameObject);

        if (WaterPlane != null)
            UnityEngine.Object.DestroyImmediate(WaterPlane);

        PaintMode = PaintType.Splat;
        SelectedTopology = TerrainTopology.Enum.Field;
    }

    public void Save(ref WorldSerialization blob)
    {
        if (!HasValidTerrain())
            return;

        blob.world.size = (uint)Terrain.terrainData.size.x;

        byte[] byteArray = ArrayUtils.FloatToByteArray(Terrain.terrainData.GetHeights(0, 0, Terrain.terrainData.heightmapWidth, Terrain.terrainData.heightmapHeight));

        blob.AddMap("terrain", byteArray);
        blob.AddMap("height", byteArray);
        blob.AddMap("water", Water.GetBytes());
        blob.AddMap("splat", Splat.GetBytes());
        blob.AddMap("alpha", Alpha.GetBytes());
        blob.AddMap("biome", Biome.GetBytes());
        blob.AddMap("topology", Topology.GetBytes());
    }

    public IEnumerator Load(World.Data world)
    {
        ActionProgressBar.UpdateProgress("Loading Terrain", 0f);
        yield return null;
        yield return null;

        Splat = new SplatMap(world.splatMap);
        Alpha = new AlphaMap(world.alphaMap);
        Biome = new BiomeMap(world.biomeMap);
        Topology = new TopologyMap(world.topologyMap);
        Water = new WaterMap(world.waterMap);

        TerrainData terrainData = new TerrainData();
        terrainData.alphamapResolution = Mathf.Clamp(Mathf.NextPowerOfTwo((int)(world.size.x * 0.50f)), 16, 2048);
        terrainData.baseMapResolution = Mathf.NextPowerOfTwo((int)((float)(world.size.x) * 0.01f));
        terrainData.heightmapResolution = Mathf.NextPowerOfTwo((int)((float)(world.size.x) * 0.5f)) + 1;

        terrainData.size = world.size;

        terrainData.SetHeights(0, 0, world.landHeightMap);

        Terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();

        Terrain.name = Terrain.tag = "Terrain";

        Terrain.gameObject.layer = 8;

        Terrain.transform.position = -0.5f * terrainData.size;

        Terrain.gameObject.AddComponent<PositionLock>();

        SetSplatMaps(PaintType.Splat);

        CreateWaterPlane(world.size.x);
    }

    private void CreateWaterPlane(float terrainBounds)
    {
        if (WaterPlane == null)
        {
            WaterPlane = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Water/WaterPlane.prefab"));
            WaterPlane.name = "WaterPlane";
            WaterPlane.tag = "Water";            
        }

        WaterPlane.transform.localPosition = Vector3.zero;
        WaterPlane.transform.localScale = new Vector3(terrainBounds, 1, terrainBounds) / 10f;
        WaterPlane.transform.localEulerAngles = Vector3.zero;

        WaterPlane.transform.position = Vector3.zero;

        WaterPlane.AddComponent<PositionLock>();
    }

    public void ChangePaintType(PaintType paintType)
    {
        if (paintType == PaintMode)
            return;

        if (!HasValidTerrain())
            return;

        StoreCurrentAlphaMaps();
        SetSplatMaps(paintType);
    }

    public void ChangeTopologyLayer(TerrainTopology.Enum topology)
    {
        if (topology == SelectedTopology)
            return;

        if (!HasValidTerrain())
            return;

        StoreCurrentAlphaMaps();

        SelectedTopology = topology;

        SetSplatMaps(PaintType.Topology);
    }

    public static bool HasValidTerrain()
    {
        if (Terrain == null)
        {
            Terrain = GameObject.FindGameObjectWithTag("Terrain")?.GetComponent<Terrain>();
            if (Terrain == null)
            {
                return false;
            }
        }

        if (WaterPlane == null)        
            WaterPlane = GameObject.FindGameObjectWithTag("Water");
        
        return true;
    }

    protected void StoreCurrentAlphaMaps()
    {
        switch (PaintMode)
        {
            case PaintType.Splat:
                Splat.UpdateFromTerrain(Terrain);
                break;
            case PaintType.Biome:
                Biome.UpdateFromTerrain(Terrain);
                break;
            case PaintType.Alpha:
                Alpha.UpdateFromTerrain(Terrain);
                break;
            case PaintType.Topology:
                Topology.UpdateFromTerrain(SelectedTopology, Terrain);
                return;
        }
    }

    protected void SetSplatMaps(PaintType paintType)
    {
        TerrainLayer[] terrainLayers = GetTerrainLayerArray((int)paintType);
        float[,,] alphaMaps = null;

        switch (paintType)
        {
            case PaintType.Splat:
                alphaMaps = Splat.MapArray;
                break;
            case PaintType.Biome:
                alphaMaps = Biome.MapArray;
                break;
            case PaintType.Alpha:
                alphaMaps = Alpha.MapArray;
                break;
            case PaintType.Topology:
                alphaMaps = Topology.GetArray(SelectedTopology);
                break;
        }

        PaintMode = paintType;

        Terrain.terrainData.terrainLayers = terrainLayers;
        Terrain.terrainData.SetAlphamaps(0, 0, alphaMaps);
        Terrain.terrainData.SetBaseMapDirty();
    }

    protected TerrainLayer[] GetTerrainLayerArray(int index)
    {
        TerrainLayerConfig terrainLayerConfig = AssetDatabase.LoadAssetAtPath<TerrainLayerConfig>(terrainLayerArrays[index]);
        return terrainLayerConfig.TerrainLayers;
    }

    protected string[] terrainLayerArrays = new string[]
    {
        "Assets/Resources/SplatArray.asset",
        "Assets/Resources/BiomeArray.asset",
        "Assets/Resources/AlphaArray.asset",
        "Assets/Resources/TopologyArray.asset",
    };
}

public enum PaintType { Splat, Biome, Alpha, Topology }
   
