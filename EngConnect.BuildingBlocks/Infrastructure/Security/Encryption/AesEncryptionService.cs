using System.Security.Cryptography;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace EngConnect.BuildingBlock.Infrastructure.Security.Encryption;

public class AesEncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public AesEncryptionService(IOptions<EncryptionSettings> encryptionSettings)
    {
        // Convert the key and IV from Base64 to byte arrays
        try
        {
            _key = Convert.FromBase64String(encryptionSettings.Value.Key);
            _iv = Convert.FromBase64String(encryptionSettings.Value.IV);
            
            // Validate key and IV lengths
            if (_key.Length != 32) // 256 bits = 32 bytes
            {
                throw new ArgumentException("Encryption key must be 256 bits (32 bytes)");
            }
            
            if (_iv.Length != 16) // 128 bits = 16 bytes
            {
                throw new ArgumentException("Encryption IV must be 128 bits (16 bytes)");
            }
        }
        catch (FormatException)
        {
            throw new ArgumentException("Encryption key and IV must be valid Base64 strings");
        }
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            return plainText;
        }

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(plainText);
        }

        var encryptedBytes = memoryStream.ToArray();
        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return cipherText;
        }

        byte[] cipherBytes;
        try
        {
            cipherBytes = Convert.FromBase64String(cipherText);
        }
        catch (FormatException)
        {
            // If the input is not a valid Base64 string, return it as is
            // This helps handle cases where data was stored unencrypted
            return cipherText;
        }

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(cipherBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        
        return streamReader.ReadToEnd();
    }
}