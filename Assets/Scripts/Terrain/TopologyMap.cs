using UnityEngine;

public class TopologyMap : TerrainMap<int>
{
    public const int CHANNELS = 31;

    public TopologyMap(TerrainMap<int> intMap)
    {
        this.res = intMap.res;
        this.src = this.dst = intMap.dst;
    }

    public float[,,] GetArray(TerrainTopology.Enum layer)
    {
        float[,,] splats = new float[res, res, 2];

        for (int z = 0; z < res; z++)
        {
            for (int x = 0; x < res; x++)
            {
                if ((this[z, x] & (int)layer) != 0)
                {
                    splats[z, x, 0] = 1f;
                    splats[z, x, 1] = 0f;
                }
                else
                {
                    splats[z, x, 0] = 0f;
                    splats[z, x, 1] = 1f;
                }
            }
        }

        return splats;
    }

    public void SetArray(TerrainTopology.Enum layer, float[,,] splatMap)
    {
        for (int z = 0; z < res; z++)
        {
            for (int x = 0; x < res; x++)
            {
                bool set = (splatMap[z, x, 0] == 1f);

                if (set)
                    this[z, x] |= (int)layer;
                else this[z, x] &= ~(int)layer;
            }
        }
    }

    public void UpdateFromTerrain(TerrainTopology.Enum layer, Terrain terrain)
    {
        SetArray(layer, terrain.terrainData.GetAlphamaps(0, 0, res, res));
    }

    public byte[] GetBytes() => ToByteArray();
}
