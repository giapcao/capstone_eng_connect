using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class CourseModuleErrors
{
    public static Error CourseSessionNotFound() => 
        new Error("CourseSession.NotFound", "Phải có ít nhất 1 buổi học trong một module.");
    
    public static Error RelationshipExist() =>
        new Error("CourseModule.RelationshipExist", "Module đã tồn tại trong một khóa học");
}