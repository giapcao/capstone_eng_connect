using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class CourseSessionErrors
{
    public static Error CourseResourceNotFound() => 
        new Error("CourseSession.CourseResourceNotFound", "Phải có ít nhất 1 tài nguyên trong 1 buổi học");
    public static Error RelationshipExist() =>
        new Error("CourseSession.RelationshipExist", "Buổi học đã tồn tại trong một module"); 
    
    public static Error CourseSessionIsInUse() =>
        new Error("CourseSession.CourseSessionIsInUse", "Buổi học đang được sử dụng trong một khóa học đang xuất bản, không thể cập nhật.");
}