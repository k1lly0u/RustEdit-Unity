using System.Collections.Generic;
using UnityEngine;

public class World
{
    public struct Data
    {
        public int resolution;
        public Vector3 size;
        public TerrainMap<short> terrainMap;
        public TerrainMap<byte> splatMap;
        public TerrainMap<byte> biomeMap;
        public TerrainMap<byte> alphaMap;
        public TerrainMap<int> topologyMap;
        public TerrainMap<short> waterMap;
        public float[,] landHeightMap;
        public List<ProtoBuf.PrefabData> prefabData;
        public List<ProtoBuf.PathData> pathData;
        
        public void Clear()
        {
            terrainMap = null;
            splatMap = null;
            biomeMap = null;
            alphaMap = null;
            topologyMap = null;
            waterMap = null;
            landHeightMap = null;
            prefabData.Clear();
            pathData.Clear();
        }
    }

    public static Data WorldToTerrain(WorldSerialization blob)
    {
        Data worldData = new Data();

        Vector3 terrainSize = new Vector3(blob.world.size, 1000, blob.world.size);

        worldData.terrainMap = new TerrainMap<short>(blob.GetMap("terrain").data, 1);

        TerrainMap<short> heightMap = new TerrainMap<short>(blob.GetMap("height").data, 1);

        worldData.splatMap = new TerrainMap<byte>(blob.GetMap("splat").data, 8);
        worldData.alphaMap = new TerrainMap<byte>(blob.GetMap("alpha").data, 1);
        worldData.biomeMap = new TerrainMap<byte>(blob.GetMap("biome").data, 4);
        worldData.topologyMap = new TerrainMap<int>(blob.GetMap("topology").data, 1);
        worldData.waterMap = new TerrainMap<short>(blob.GetMap("water").data, 1);

        worldData.pathData = new List<ProtoBuf.PathData>(blob.world.paths);
        worldData.prefabData = new List<ProtoBuf.PrefabData>(blob.world.prefabs);

        worldData.resolution = heightMap.res;
        worldData.size = terrainSize;

        Debug.Log($"Load : World Size {terrainSize.x} Heightmap Res {heightMap.res} Texture Res {worldData.splatMap.res}");

        worldData.landHeightMap = ArrayUtils.ShortToFloatArray(worldData.terrainMap);

        blob.Clear();

        return worldData;
    }

    public static Data NewDataFromSize(int mapSize, int splatType, int biomeType)
    {
        Data worldData = new Data();

        worldData.pathData = new List<ProtoBuf.PathData>();
        worldData.prefabData = new List<ProtoBuf.PrefabData>();

        worldData.resolution = Mathf.NextPowerOfTwo((int)(mapSize * 0.50f)) + 1;
        worldData.size = new Vector3(mapSize, 1000, mapSize);

        int textureResolution = Mathf.Clamp(Mathf.NextPowerOfTwo((int)(mapSize * 0.50f)), 16, 2048);
        Debug.Log($"New : World Size {mapSize} Heightmap Res {worldData.resolution} Texture Res {textureResolution}");
        worldData.landHeightMap = ArrayUtils.CreateTerrainMap(worldData.resolution, 0.5f);
        worldData.waterMap = ArrayUtils.CreateNewShortMap(worldData.resolution, 0.5f);
        worldData.terrainMap = new TerrainMap<short>(ArrayUtils.FloatToByteArray(worldData.landHeightMap), 1);

        worldData.splatMap = ArrayUtils.CreateNewByteMap(textureResolution, 8, splatType);
        worldData.alphaMap = ArrayUtils.CreateNewByteMap(textureResolution, 1, 0);
        worldData.biomeMap = ArrayUtils.CreateNewByteMap(textureResolution, 4, biomeType);
        worldData.topologyMap = new TerrainMap<int>(textureResolution, 1);

        return worldData;
    }
}
