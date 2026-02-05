using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Lessons.GetLessonById;

public record GetLessonByIdQuery : IQuery<GetLessonResponse>
{
    public Guid Id { get; set; }
}
