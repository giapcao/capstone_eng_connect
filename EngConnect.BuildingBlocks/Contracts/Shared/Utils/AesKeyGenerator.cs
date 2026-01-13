using System.Security.Cryptography;

namespace EngConnect.BuildingBlock.Contracts.Shared.Utils;

public static class AesKeyGenerator
{
    public static (string key, string iv) GenerateKeyAndIv()
    {
        using var aes = Aes.Create();
        aes.KeySize = 256; // AES-256
        aes.GenerateKey();
        aes.GenerateIV();
        
        var key = Convert.ToBase64String(aes.Key);
        var iv = Convert.ToBase64String(aes.IV);
        
        return (key, iv);
    }
}