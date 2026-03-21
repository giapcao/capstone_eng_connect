using Microsoft.AspNetCore.Http;

namespace EngConnect.Application.Common;

public class FileFormatRequest
{
    public IFormFile File { get; set; } = null!;
    public string FileName { get; set; } = null!;

}