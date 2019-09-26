public class WaterMap : TerrainMap<short>
{
    private float[,] _mapArray;

    public float[,] MapArray { get { return _mapArray; } }

    public WaterMap(TerrainMap<short> byteMap)
    {
        res = byteMap.res;

        _mapArray = ArrayUtils.ShortToFloatArray(byteMap);
    }

    public byte[] GetBytes()
    {
        return ArrayUtils.FloatToByteArray(_mapArray);
    }
}
