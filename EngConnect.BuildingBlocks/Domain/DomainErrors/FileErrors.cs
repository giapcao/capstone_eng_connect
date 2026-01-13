using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Domain.DomainErrors;

public static class FileErrors
{
    public const string FileNotFoundErrorCode = "File.NotFound";
    public static Error NotFound(string path)
    {
        return new Error(FileNotFoundErrorCode, $"Không tìm thấy file tại đường dẫn: {path}");
    }

    public static Error InvalidPath(string message)
    {
        return new Error("File.InvalidPath", message);
    }

    public static Error UnsupportedFileType(string extension)
    {
        return new Error("File.UnsupportedFileType", $"Loại file không được hỗ trợ: {extension}");
    }

    public static Error ExceededMaxSize(long maxSizeInBytes)
    {
        return new Error("File.ExceedsMaxSize", $"Kích thước file vượt quá giới hạn cho phép: {maxSizeInBytes / 1024 / 1024}MB");
    }
}
