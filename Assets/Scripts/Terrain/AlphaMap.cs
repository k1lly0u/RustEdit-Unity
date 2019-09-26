using UnityEngine;

public class AlphaMap : TerrainMap<byte>
{
    private float[,,] _mapArray;

    public float[,,] MapArray { get { return _mapArray; } }

    public const int CHANNELS = 2;
   
    public AlphaMap(TerrainMap<byte> byteMap)
    {
        res = byteMap.res;

        _mapArray = ToSplatMap(byteMap);
    }

    public float[,,] ToSplatMap(TerrainMap<byte> byteMap)
    {
        float[,,] alphaMap = new float[byteMap.res, byteMap.res, CHANNELS];
        for (int z = 0; z < alphaMap.GetLength(0); z++)
        {
            for (int x = 0; x < alphaMap.GetLength(1); x++)
            {
                if (byteMap[0, z, x] > 0)
                {
                    alphaMap[z, x, 0] = 0;
                    alphaMap[z, x, 1] = 1;
                }
                else
                {
                    alphaMap[z, x, 0] = 1;
                    alphaMap[z, x, 1] = 0;
                }
            }
        }
        return alphaMap;
    }

    private TerrainMap<byte> FromSplatMap(float[,,] splatMap)
    {
        TerrainMap<byte> byteMap = new TerrainMap<byte>(res, 1);
        for (int z = 0; z < byteMap.res; z++)
        {
            for (int x = 0; x < byteMap.res; x++)
            {
                byteMap[0, z, x] = BitUtility.Float2Byte(splatMap[z, x, 1]);
            }
        }
        return byteMap;
    }

    public void UpdateFromTerrain(Terrain terrain)
    {
        _mapArray = terrain.terrainData.GetAlphamaps(0, 0, res, res);
    }

    public byte[] GetBytes()
    {
        TerrainMap<byte> byteMap = FromSplatMap(_mapArray);
        return byteMap.ToByteArray();
    }
}
