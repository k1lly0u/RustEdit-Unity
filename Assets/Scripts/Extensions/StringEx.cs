using System;
using System.Text;
using System.Security.Cryptography;

public static class StringEx
{
    public static uint ManifestHash(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return 0u;
        }
        str = str.ToLower();
        MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
        byte[] value = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(str));
        return BitConverter.ToUInt32(value, 0);
    }
}
