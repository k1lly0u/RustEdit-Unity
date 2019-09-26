public class HeightMap : TerrainMap<short>
{
    private float[,] _mapArray;

    public float[,] MapArray { get { return _mapArray; } }

    public HeightMap(TerrainMap<short> shortMap)
    {
        res = shortMap.res;

        _mapArray = ToHeightMap(shortMap);
    }

    public float[,] ToHeightMap(TerrainMap<short> shortMap)
    {
        float[,] floatMap = new float[shortMap.res, shortMap.res];
        for (int z = 0; z < floatMap.GetLength(0); z++)
        {
            for (int x = 0; x < floatMap.GetLength(1); x++)
            {
                floatMap[z, x] = BitUtility.Short2Float(shortMap[z, x]);

            }
        }
        return floatMap;
    }
}
