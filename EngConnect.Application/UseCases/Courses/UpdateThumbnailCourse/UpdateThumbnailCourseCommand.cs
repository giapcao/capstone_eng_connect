using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.Courses.UpdateThumbnailCourse;

public class UpdateThumbnailCourseCommand : ICommand
{
    public required FileUpload File { get; set; }
    public required Guid Id { get; set; }
}
