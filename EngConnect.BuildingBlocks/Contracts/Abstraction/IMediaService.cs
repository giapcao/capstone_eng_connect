using EngConnect.BuildingBlock.Contracts.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IMediaService
{
    /// <summary>
    ///     Uploads an image to Cloudinary with a folder structure
    /// </summary>
    /// <param name="file">The image file from form upload</param>
    /// <param name="directory"></param>
    /// <returns>Media upload result containing URL and PublicId</returns>
    Task<Result<string>> UploadImageAsync(IFormFile file, string directory);

    /// <summary>
    ///     Uploads a video to Cloudinary with a folder structure
    /// </summary>
    /// <param name="file">The video file from form upload</param>
    /// <param name="directory"></param>
    /// <returns>Media upload result containing URL and PublicId</returns>
    Task<Result<string>> UploadVideoAsync(IFormFile file, string directory);

    /// <summary>
    ///     Deletes a media asset from Cloudinary
    /// </summary>
    /// <param name="publicId">The public ID of the media to delete</param>
    /// <returns>True if deletion was successful</returns>
    Task<ValidationResultExtensions.Result> DeleteMediaAsync(string publicId);
}