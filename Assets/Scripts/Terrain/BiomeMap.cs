using UnityEngine;

public class BiomeMap : TerrainMap<byte>
{
    private float[,,] _mapArray;

    public float[,,] MapArray { get { return _mapArray; } }

    public const int CHANNELS = 4;

    public BiomeMap(TerrainMap<byte> byteMap)
    {
        res = byteMap.res;

        _mapArray = ToSplatMap(byteMap);
    }

    public float[,,] ToSplatMap(TerrainMap<byte> byteMap)
    {
        float[,,] biomeMap = new float[byteMap.res, byteMap.res, CHANNELS];
        for (int z = 0; z < biomeMap.GetLength(0); z++)
        {
            for (int x = 0; x < biomeMap.GetLength(1); x++)
            {
                for (int c = 0; c < CHANNELS; c++)
                {
                    biomeMap[z, x, c] = BitUtility.Byte2Float(byteMap[c, z, x]);
                }
            }
        }
        return biomeMap;
    }

    private TerrainMap<byte> FromSplatMap(float[,,] splatMap)
    {
        TerrainMap<byte> byteMap = new TerrainMap<byte>(res, CHANNELS);
        for (int z = 0; z < byteMap.res; z++)
        {
            for (int x = 0; x < byteMap.res; x++)
            {
                for (int c = 0; c < CHANNELS; c++)
                {
                    byteMap[c, z, x] = BitUtility.Float2Byte(splatMap[z, x, c]);
                }
            }
        }
        return byteMap;
    }

    public byte[] GetBytes()
    {
        TerrainMap<byte> byteMap = FromSplatMap(_mapArray);
        return byteMap.ToByteArray();
    }

    public void UpdateFromTerrain(Terrain terrain)
    {
        _mapArray = terrain.terrainData.GetAlphamaps(0, 0, res, res);
    }
}
