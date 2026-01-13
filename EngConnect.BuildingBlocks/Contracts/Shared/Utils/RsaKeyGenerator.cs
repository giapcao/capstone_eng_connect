using System.Security.Cryptography;
using System.Text;

namespace EngConnect.BuildingBlock.Contracts.Shared.Utils;

public static class RsaKeyGenerator
{
    public static (string privateKey, string publicKey) GenerateKeyPair()
    {
        using var rsa = RSA.Create(2048); // 2048-bit key

        var privateKey = rsa.ExportRSAPrivateKeyPem();
        var publicKey = rsa.ExportRSAPublicKeyPem();

        return (privateKey, publicKey);
    }

    /// <summary>
    ///     Read RSA key from Base64 string
    /// </summary>
    /// <param name="encodedKey"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static RSA ReadRsaKeyBase64(this string encodedKey)
    {
        if (string.IsNullOrWhiteSpace(encodedKey))
        {
            throw new Exception("RSA key cannot be null or empty.");
        }

        var rsa = RSA.Create();
        try
        {
            // Remove all whitespace characters (spaces, tabs, newlines, carriage returns)
            var cleanedKey = new string(encodedKey.Where(c => !char.IsWhiteSpace(c)).ToArray());
            
            // Add padding if necessary (Base64 strings must be a multiple of 4)
            int padding = cleanedKey.Length % 4;
            if (padding > 0)
            {
                cleanedKey += new string('=', 4 - padding);
            }
            
            // Try to decode Base64 first
            var keyBytes = Convert.FromBase64String(cleanedKey);
            var keyPem = Encoding.UTF8.GetString(keyBytes);
            rsa.ImportFromPem(keyPem);
        }
        catch (FormatException ex)
        {
            throw new Exception($"Invalid key format. The key must be a valid Base64 encoded PEM string. Error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to import RSA key: {ex.Message}", ex);
        }

        return rsa;
    }
}