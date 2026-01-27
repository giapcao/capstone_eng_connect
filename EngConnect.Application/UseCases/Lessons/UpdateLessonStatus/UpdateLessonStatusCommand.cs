using EngConnect.BuildingBlock.Application.Base;
using EngConnect.Domain.Constants;

namespace EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;

public class UpdateLessonStatusCommand : ICommand
{
    public Guid Id { get; set; }
    
    public string? Status { get; set; }
}
