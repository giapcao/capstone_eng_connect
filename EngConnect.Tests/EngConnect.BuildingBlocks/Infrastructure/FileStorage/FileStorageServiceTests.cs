using System.Text;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Infrastructure.FileStorage;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace EngConnect.Tests.EngConnect.BuildingBlocks.Infrastructure.FileStorage;

public class FileStorageServiceTests
{
    [Fact]
    public async Task Upload_get_and_delete_round_trip_public_file()
    {
        var root = Path.Combine(Path.GetTempPath(), "engconnect-tests", Guid.NewGuid().ToString("N"));
        var sut = CreateService(root);

        try
        {
            var uploadResult = await sut.UploadFileAsync(new FileUpload
            {
                FileName = "profile.txt",
                ContentType = "text/plain",
                Length = 5,
                Content = new MemoryStream(Encoding.UTF8.GetBytes("hello"))
            }, isPrivate: false, directory: "docs");

            Assert.True(uploadResult.IsSuccess);
            Assert.NotNull(uploadResult.Value);

            var getResult = sut.GetFile(uploadResult.Value!.RelativePath, isPrivate: false);

            Assert.True(getResult.IsSuccess);
            Assert.NotNull(getResult.Value);

            using (getResult.Value!.Stream)
            {
                Assert.Equal("profile.txt", getResult.Value.FileName);
            }

            var deleteResult = sut.DeleteFile(uploadResult.Value.RelativePath, isPrivate: false);
            Assert.True(deleteResult.IsSuccess);
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Fact]
    public void ValidateFile_rejects_unsupported_extensions()
    {
        var root = Path.Combine(Path.GetTempPath(), "engconnect-tests", Guid.NewGuid().ToString("N"));
        var sut = CreateService(root);

        try
        {
            var result = sut.ValidateFile(new FileUpload
            {
                FileName = "malware.exe",
                ContentType = "application/octet-stream",
                Length = 4,
                Content = new MemoryStream([1, 2, 3, 4])
            });

            Assert.False(result.IsSuccess);
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    private static FileStorageService CreateService(string root)
    {
        var settings = new FileStorageSettings
        {
            BaseUrl = "https://files.example",
            MaxFileSize = 1024 * 1024,
            UseUniqueFilenames = false,
            PublicFileBasePath = Path.Combine(root, "public"),
            PrivateFileBasePath = Path.Combine(root, "private")
        };

        return new FileStorageService(Options.Create(settings), NullLogger<FileStorageService>.Instance);
    }
}
