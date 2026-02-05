using EngConnect.Application.UseCases.LessonScripts.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;

public record GetListLessonScriptsQuery : BaseQuery<PaginationResult<GetLessonScriptResponse>>
{
    public Guid? LessonId { get; set; }
    
    public Guid? RecordId { get; set; }
    
    public string? Language { get; set; }
}
