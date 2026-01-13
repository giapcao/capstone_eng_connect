using System.Security.Cryptography;
using System.Text;

namespace EngConnect.BuildingBlock.Contracts.Shared;

public static class HashHelper
{
    public static string GenerateSha256Key(params string[] parts)
    {
        var concatenated = string.Join(":", parts);
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(concatenated));
        var sb = new StringBuilder();
        foreach (var b in hashBytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}