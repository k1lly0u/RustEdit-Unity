using System;
using UnityEngine;

public class ArrayUtils
{
    public static TerrainMap<byte> CreateNewByteMap(int size, int channels, int target)
    {
        TerrainMap<byte> byteMap = new TerrainMap<byte>(size, channels);

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    if (c == target)
                        byteMap[c, z, x] = BitUtility.Float2Byte(1);
                    else byteMap[c, z, x] = BitUtility.Float2Byte(0);
                }
            }
        }
        return byteMap;
    }

    public static TerrainMap<short> CreateNewShortMap(int size, float target)
    {
        TerrainMap<short> byteMap = new TerrainMap<short>(size, 1);

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                byteMap[z, x] = BitUtility.Float2Short(target);               
            }
        }
        return byteMap;
    }

    public static float[,] CreateTerrainMap(int size, float height)
    {
        float[,] terrainMap = new float[size, size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                terrainMap[z, x] = height;
            }
        }
        return terrainMap;
    }

    public static float[,] ShortToFloatArray(TerrainMap<short> shortmap)
    {
        float[,] array = new float[shortmap.res, shortmap.res];
        for (int z = 0; z < array.GetLength(0); z++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                array[z, x] = BitUtility.Short2Float(shortmap[z, x]);
            }
        }
        return array;
    }

    public static float[,] ShortToFloatArray(byte[] byteArray, int res)
    {
        float[,] array = new float[res, res];
        for (int z = 0; z < array.GetLength(0); z++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                array[z, x] = BitUtility.Short2Float(byteArray[z * res + x]);
            }
        }
        return array;
    }

    public static float[,] ByteTo2DArray(byte[] byteArray, int res)
    {
        float[,] array = new float[res, res];
        for (int z = 0; z < array.GetLength(0); z++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                array[z, x] = BitUtility.Byte2Float(byteArray[z * res + x]);
            }
        }
        return array;
    }

    public static float[,,] ByteTo3DArray(byte[] byteArray, int res, int channels)
    {
        float[,,] array = new float[res, res, channels];
        for (int z = 0; z < res; z++)
        {
            for (int x = 0; x < res; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    array[z, x, c] = BitUtility.Byte2Float(byteArray[(c * res + z) * res + x]);
                }
            }
        }
        return array;
    }

    public static byte[] FloatToByteArray(float[,] floatArray)
    {
        short[] shortArray = new short[floatArray.GetLength(0) * floatArray.GetLength(1)];

        for (int z = 0; z < floatArray.GetLength(0); z++)
        {
            for (int x = 0; x < floatArray.GetLength(1); x++)
            {
                shortArray[(z * floatArray.GetLength(0)) + x] = BitUtility.Float2Short(floatArray[z, x]);
            }
        }

        byte[] byteArray = new byte[shortArray.Length * 2];

        Buffer.BlockCopy(shortArray, 0, byteArray, 0, byteArray.Length);

        return byteArray;
    }

    public static T[] MultiToSingleArray<T>(T[,,] multiArray, int size)
    {
        T[] singleArray = new T[multiArray.GetLength(0) * multiArray.GetLength(1) * size];

        for (int z = 0; z < multiArray.GetLength(0); z++)
        {
            for (int x = 0; x < multiArray.GetLength(1); x++)
            {
                for (int c = 0; c < size; c++)
                {
                    singleArray[z * multiArray.GetLength(1) * size + (x * size + c)] = multiArray[z, x, c];
                }
            }
        }

        return singleArray;
    }

    public static T[] MultiToSingleArraySpecificChannel<T>(T[,,] multiArray, int channel)
    {
        T[] singleArray = new T[multiArray.GetLength(0) * multiArray.GetLength(1)];

        for (int z = 0; z < multiArray.GetLength(0); z++)
        {
            for (int x = 0; x < multiArray.GetLength(1); x++)
            {
                singleArray[z * multiArray.GetLength(1) + x] = multiArray[z, x, channel];
            }
        }

        return singleArray;
    }

    public static T[] DoubleToSingleArray<T>(T[,] multiArray)
    {
        T[] singleArray = new T[multiArray.GetLength(0) * multiArray.GetLength(1)];

        for (int z = 0; z < multiArray.GetLength(0); z++)
        {
            for (int x = 0; x < multiArray.GetLength(1); x++)
            {
                singleArray[z * multiArray.GetLength(1) + x] = multiArray[z, x];
            }
        }

        return singleArray;
    }

    public static T[,] SingleToDoubleArray<T>(T[] singleArray)
    {
        int length = (int)Math.Sqrt(singleArray.Length);

        T[,] multiArray = new T[length, length];

        for (int z = 0; z < multiArray.GetLength(0); z++)
        {
            for (int x = 0; x < multiArray.GetLength(1); x++)
            {
                multiArray[z, x] = singleArray[z * multiArray.GetLength(1) + x];
            }
        }

        return multiArray;
    }

    public static T[,,] SingleToMulti<T>(T[] singleArray, int texturesAmount)
    {
        int length = (int)Math.Sqrt(singleArray.Length / texturesAmount);

        T[,,] multiArray = new T[length, length, texturesAmount];
        for (int z = 0; z < multiArray.GetLength(0); z++)
        {
            for (int x = 0; x < multiArray.GetLength(1); x++)
            {
                for (int c = 0; c < multiArray.GetLength(2); c++)
                {
                    multiArray[z, x, c] = singleArray[z * multiArray.GetLength(1) * multiArray.GetLength(2) + (x * multiArray.GetLength(2) + c)];
                }
            }
        }

        return multiArray;
    }

    public static void InsertChannelToMulti<T>(ref T[,,] source, T[] singleArray, int channel)
    {
        int length = (int)Math.Sqrt(singleArray.Length);

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < length; x++)
            {
                source[z, x, channel] = singleArray[z * length + x];
            }
        }
    }

    public static TerrainMap<byte> Resize3DArray(TerrainMap<byte> map, int channels, int targetSize)
    {
        TerrainMap<byte> byteMap = new TerrainMap<byte>(targetSize, channels);

        float modifier = (float)byteMap.res / (float)targetSize;

        for (int z = 0; z < targetSize; z++)
        {
            for (int x = 0; x < targetSize; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    int adjZ = Mathf.Clamp(Mathf.CeilToInt((float)z * modifier), 0, byteMap.res - 1);
                    int adjX = Mathf.Clamp(Mathf.CeilToInt((float)x * modifier), 0, byteMap.res - 1);

                    byteMap[c, z, x] = map[c, adjZ, adjX];
                }
            }
        }
        return byteMap;
    }

    public static TerrainMap<int> Resize3DArray(TerrainMap<int> map, int channels, int targetSize)
    {
        TerrainMap<int> byteMap = new TerrainMap<int>(targetSize, channels);

        float modifier = (float)byteMap.res / (float)targetSize;

        for (int z = 0; z < targetSize; z++)
        {
            for (int x = 0; x < targetSize; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    int adjZ = Mathf.Clamp(Mathf.CeilToInt((float)z * modifier), 0, byteMap.res - 1);
                    int adjX = Mathf.Clamp(Mathf.CeilToInt((float)x * modifier), 0, byteMap.res - 1);
                    byteMap[c, z, x] = map[c, adjZ, adjX];
                }
            }
        }
        return byteMap;
    }

    public static T[,,] ResizeArray<T>(T[,,] source, int currentSize, int targetSize)
    {
        if (currentSize == targetSize)
            return source;

        T[,,] target = new T[targetSize, targetSize, source.GetLength(2)];

        float modifier = (float)currentSize / (float)targetSize;

        for (int z = 0; z < targetSize; z++)
        {
            for (int x = 0; x < targetSize; x++)
            {
                for (int c = 0; c < source.GetLength(2); c++)
                {
                    int adjZ = Mathf.Clamp(Mathf.CeilToInt((float)z * modifier), 0, currentSize - 1);
                    int adjX = Mathf.Clamp(Mathf.CeilToInt((float)x * modifier), 0, currentSize - 1);
                    target[z, x, c] = source[adjZ, adjX, c];
                }
            }
        }
        return target;
    }

    public static T[,] ResizeArray<T>(T[,] source, int currentSize, int targetSize)
    {
        if (currentSize == targetSize)
            return source;

        T[,] target = new T[targetSize, targetSize];

        float modifier = (float)currentSize / (float)targetSize;

        for (int z = 0; z < targetSize; z++)
        {
            for (int x = 0; x < targetSize; x++)
            {
                int adjZ = Mathf.Clamp(Mathf.CeilToInt((float)z * modifier), 0, currentSize - 1);
                int adjX = Mathf.Clamp(Mathf.CeilToInt((float)x * modifier), 0, currentSize - 1);
                target[z, x] = source[adjZ, adjX];
            }
        }
        return target;
    }

    public static T[,,] ResizeArray<T>(T[,,] source, int currentX, int currentZ, int targetX, int targetZ)
    {
        if (currentX == targetX && currentZ == targetZ)
            return source;

        T[,,] target = new T[targetZ, targetX, source.GetLength(2)];

        float modifierX = (float)currentX / (float)targetX;
        float modifierZ = (float)currentZ / (float)targetZ;

        for (int z = 0; z < targetZ; z++)
        {
            for (int x = 0; x < targetX; x++)
            {
                for (int c = 0; c < source.GetLength(2); c++)
                {
                    int adjZ = Mathf.Clamp(Mathf.CeilToInt((float)z * modifierZ), 0, currentZ - 1);
                    int adjX = Mathf.Clamp(Mathf.CeilToInt((float)x * modifierX), 0, currentX - 1);
                    target[z, x, c] = source[adjZ, adjX, c];
                }
            }
        }
        return target;
    }

    public static T[,] ResizeArray<T>(T[,] source, int currentX, int currentZ, int targetX, int targetZ)
    {
        if (currentX == targetX && currentZ == targetZ)
            return source;

        T[,] target = new T[targetZ, targetX];

        float modifierX = (float)currentX / (float)targetX;
        float modifierZ = (float)currentZ / (float)targetZ;

        for (int z = 0; z < targetZ; z++)
        {
            for (int x = 0; x < targetX; x++)
            {
                int adjZ = Mathf.Clamp(Mathf.CeilToInt((float)z * modifierZ), 0, currentZ - 1);
                int adjX = Mathf.Clamp(Mathf.CeilToInt((float)x * modifierX), 0, currentX - 1);
                target[z, x] = source[adjZ, adjX];
            }
        }
        return target;
    }


    public static T[,] RotateCCW<T>(T[,] array)
    {
        int zLength = array.GetLength(0);
        int xLength = array.GetLength(1);
        T[,] tempArray = new T[array.GetLength(0), array.GetLength(1)];

        for (int z = 0; z < array.GetLength(0); z++)
        {
            for (int x = 0; x < array.GetLength(1); x++)
            {
                tempArray[z, x] = array[array.GetLength(0) - x - 1, z];
            }
        }

        return array = tempArray;
    }

    public static T[,,] RotateCCW<T>(T[,,] array)
    {
        int zLength = array.GetLength(0);
        int xLength = array.GetLength(1);
        int channels = array.GetLength(2);

        T[,,] tempArray = new T[zLength, xLength, channels];

        for (int z = 0; z < zLength; z++)
        {
            for (int x = 0; x < xLength; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    tempArray[z, x, c] = array[zLength - x - 1, z, c];
                }
            }
        }

        return array = tempArray;
    }

    public static T[,] RotateCW<T>(T[,] array)
    {
        int zLength = array.GetLength(0);
        int xLength = array.GetLength(1);

        T[,] tempArray = new T[zLength, xLength];

        for (int z = 0; z < zLength; z++)
        {
            for (int x = 0; x < xLength; x++)
            {
                tempArray[z, x] = array[x, xLength - z - 1];
            }
        }

        return array = tempArray;
    }

    public static T[,,] RotateCW<T>(T[,,] array)
    {
        int zLength = array.GetLength(0);
        int xLength = array.GetLength(1);
        int channels = array.GetLength(2);

        T[,,] tempArray = new T[zLength, xLength, channels];

        for (int z = 0; z < zLength; z++)
        {
            for (int x = 0; x < xLength; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    tempArray[z, x, c] = array[x, xLength - z - 1, c];
                }
            }
        }

        return array = tempArray;
    }

    public static T[,] Invert<T>(T[,] array)
    {
        int zLength = array.GetLength(0);
        int xLength = array.GetLength(1);

        T[,] tempArray = new T[zLength, xLength];

        for (int z = 0; z < zLength; z++)
        {
            for (int x = 0; x < xLength; x++)
            {
                tempArray[z, x] = array[z, zLength - x - 1];
            }
        }

        return array = tempArray;
    }

    public static T[,,] Invert<T>(T[,,] array)
    {
        int zLength = array.GetLength(0);
        int xLength = array.GetLength(1);
        int channels = array.GetLength(2);

        T[,,] tempArray = new T[zLength, xLength, channels];

        for (int z = 0; z < zLength; z++)
        {
            for (int x = 0; x < xLength; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    tempArray[z, x, c] = array[z, zLength - x - 1, c];
                }
            }
        }

        return array = tempArray;
    }

    public static T[,,] InsertInto<T>(T[,,] target, T[,,] source, int insertAt, int size)
    {
        int zActual = 0;
        int xActual = 0;

        for (int z = insertAt; z < insertAt + size; z++)
        {
            xActual = 0;
            for (int x = insertAt; x < insertAt + size; x++)
            {
                for (int c = 0; c < target.GetLength(2); c++)
                {
                    target[z, x, c] = source[zActual, xActual, c];
                }
                xActual++;
            }
            zActual++;
        }
        return target;
    }

    public static T[,] InsertInto<T>(T[,] target, T[,] source, int insertAt, int size)
    {
        int zActual = 0;
        int xActual = 0;

        for (int z = insertAt; z < insertAt + size; z++)
        {
            xActual = 0;
            for (int x = insertAt; x < insertAt + size; x++)
            {
                target[z, x] = source[zActual, xActual];
                xActual++;
            }
            zActual++;
        }
        return target;
    }

    public static T[,,] InsertInto<T>(T[,,] target, T[,,] source, int insertAtX, int insertAtZ, int sizeX, int sizeZ)
    {
        int zActual = 0;
        int xActual = 0;

        int limitZ = Mathf.Clamp(insertAtZ + sizeZ, 0, target.GetLength(0));
        int limitX = Mathf.Clamp(insertAtX + sizeX, 0, target.GetLength(1));

        for (int z = insertAtZ; z < limitZ; z++)
        {
            xActual = 0;
            for (int x = insertAtX; x < limitX; x++)
            {
                for (int c = 0; c < target.GetLength(2); c++)
                {
                    target[z, x, c] = source[zActual, xActual, c];
                }
                xActual++;
            }
            zActual++;
        }
        return target;
    }

    public static T[,] InsertInto<T>(T[,] target, T[,] source, int insertAtX, int insertAtZ, int sizeX, int sizeZ)
    {
        int zActual = 0;
        int xActual = 0;

        int limitZ = Mathf.Clamp(insertAtZ + sizeZ, 0, target.GetLength(0));
        int limitX = Mathf.Clamp(insertAtX + sizeX, 0, target.GetLength(1));

        for (int z = insertAtZ; z < limitZ; z++)
        {
            xActual = 0;
            for (int x = insertAtX; x < limitX; x++)
            {
                target[z, x] = source[zActual, xActual];
                xActual++;
            }
            zActual++;
        }
        return target;
    }

    public static T[,] FillFrom<T>(T[,] source, int resolution)
    {
        T[,] target = new T[resolution, resolution];

        int offset = (resolution - source.GetLength(0)) / 2;
        int sourceRes = source.GetLength(0);

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int xIndex = x < offset ? 0 : x >= offset + sourceRes ? sourceRes - 1 : x - offset;
                int zIndex = z < offset ? 0 : z >= offset + sourceRes ? sourceRes - 1 : z - offset;

                target[z, x] = source[zIndex, xIndex];
            }
        }

        return target;
    }

    public static T[,,] FillFrom<T>(T[,,] source, int resolution)
    {
        int channels = source.GetLength(2);

        T[,,] target = new T[resolution, resolution, channels];

        int offset = (resolution - source.GetLength(0)) / 2;
        int sourceRes = source.GetLength(0);


        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int xIndex = x < offset ? 0 : x >= offset + sourceRes ? sourceRes - 1 : x - offset;
                int zIndex = z < offset ? 0 : z >= offset + sourceRes ? sourceRes - 1 : z - offset;

                for (int c = 0; c < channels; c++)
                {
                    target[z, x, c] = source[zIndex, xIndex, c];
                }
            }
        }

        return target;
    }

    public static T[,,] TakeFrom<T>(T[,,] source, int startAt, int size)
    {
        int zActual = 0;
        int xActual = 0;

        T[,,] target = new T[size, size, source.GetLength(2)];

        for (int z = startAt; z < startAt + size; z++)
        {
            xActual = 0;
            for (int x = startAt; x < startAt + size; x++)
            {
                for (int c = 0; c < source.GetLength(2); c++)
                {
                    target[zActual, xActual, c] = source[z, x, c];
                }
                xActual++;
            }
            zActual++;
        }
        return target;
    }

    public static T[,] TakeFrom<T>(T[,] source, int startAt, int size)
    {
        int zActual = 0;
        int xActual = 0;

        T[,] target = new T[size, size];

        for (int z = startAt; z < startAt + size; z++)
        {
            xActual = 0;
            for (int x = startAt; x < startAt + size; x++)
            {
                target[zActual, xActual] = source[z, x];
                xActual++;
            }
            zActual++;
        }
        return target;
    }

    public static T[,,] TakeFrom<T>(T[,,] source, int startAtX, int startAtZ, int endAtX, int endAtZ)
    {
        int zActual = 0;
        int xActual = 0;

        endAtX = Mathf.Clamp(endAtX, 0, source.GetLength(1));
        endAtZ = Mathf.Clamp(endAtZ, 0, source.GetLength(0));

        T[,,] target = new T[endAtZ - startAtZ, endAtX - startAtX, source.GetLength(2)];

        for (int z = startAtZ; z < endAtZ; z++)
        {
            xActual = 0;
            for (int x = startAtX; x < endAtX; x++)
            {
                for (int c = 0; c < source.GetLength(2); c++)
                {
                    target[zActual, xActual, c] = source[z, x, c];
                }
                xActual++;
            }
            zActual++;
        }
        return target;
    }

    public static T[,] TakeFrom<T>(T[,] source, int startAtX, int startAtZ, int endAtX, int endAtZ)
    {
        int zActual = 0;
        int xActual = 0;

        endAtX = Mathf.Clamp(endAtX, 0, source.GetLength(1));
        endAtZ = Mathf.Clamp(endAtZ, 0, source.GetLength(0));

        T[,] target = new T[endAtZ - startAtZ, endAtX - startAtX];

        for (int z = startAtZ; z < endAtZ; z++)
        {
            xActual = 0;
            for (int x = startAtX; x < endAtX; x++)
            {
                target[zActual, xActual] = source[z, x];
                xActual++;
            }
            zActual++;
        }
        return target;
    }
}