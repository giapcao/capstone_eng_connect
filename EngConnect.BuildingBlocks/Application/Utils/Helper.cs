using System.Text;

namespace EngConnect.BuildingBlock.Application.Utils;

public static class Helper
{
    public static string EncodeCompoundCursor(DateTime createdAt, Guid id)
    {
        var dtBytes = BitConverter.GetBytes(createdAt.ToBinary());
        var guidBytes = id.ToByteArray();
        var combined = dtBytes.Concat(guidBytes).ToArray();
        return Convert.ToBase64String(combined);
    }

    public static (DateTime CreatedAt, Guid Id) DecodeCompoundCursor(string cursor)
    {
        var bytes = Convert.FromBase64String(cursor);
        var dtBinary = BitConverter.ToInt64(bytes, 0);
        var createdAt = DateTime.FromBinary(dtBinary);
        var guidBytes = bytes.Skip(8).Take(16).ToArray();
        var id = new Guid(guidBytes);
        return (createdAt, id);
    }

    /// <summary>
    ///     encode the plain text to base64 string
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string EncodeBase64(string? plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText ?? string.Empty);
        return Convert.ToBase64String(plainTextBytes);
    }

    /// <summary>
    ///     decode the base64 string to plain text
    /// </summary>
    /// <param name="base64EncodedData"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string DecodeBase64(string? base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData ?? string.Empty);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static bool IsValidRtspPrefix(string url)
    {
        return url.StartsWith("rtsp://");
    }
}