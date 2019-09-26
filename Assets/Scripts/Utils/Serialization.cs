using System.IO;
using ProtoBuf;

public static class Serialization
{
    public static byte[] Serialize<T>(T obj) where T : class
    {
        try
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    Serializer.Serialize(memoryStream, obj);
                    return memoryStream.ToArray();
                }
            }
        }
        catch
        {
            return null;
        }
    }

    public static T Deserialize<T>(byte[] data) where T : class
    {
        try
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    return Serializer.Deserialize<T>(memoryStream);
                }
            }
        }
        catch 
        {
            return null;
        }
    }
}
