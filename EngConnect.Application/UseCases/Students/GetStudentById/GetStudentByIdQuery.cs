using EngConnect.Application.UseCases.Students.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Students.GetStudentById;

public record GetStudentByIdQuery : IQuery<GetStudentResponse>
{
    public Guid Id {get; set; }
}