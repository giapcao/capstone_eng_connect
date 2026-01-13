namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IEncryptionService
{
    /// <summary>
    ///     Encrypts a string using AES-256
    /// </summary>
    /// <param name="plainText">The text to encrypt</param>
    /// <returns>Base64 encoded encrypted string</returns>
    string Encrypt(string plainText);

    /// <summary>
    ///     Decrypts an AES-256 encrypted string
    /// </summary>
    /// <param name="cipherText">Base64 encoded encrypted string</param>
    /// <returns>The decrypted text</returns>
    string Decrypt(string cipherText);
}