using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class CourseSessionErrors
{
    public static Error CourseResourceNotFound() => 
        new Error("CourseSession.CourseResourceNotFound", "Phải có ít nhất 1 tài nguyên trong 1 buổi học");
}