using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class CourseResourceErrors
{
    public static Error RelationshipExist() =>
        new Error("CourseResource.RelationshipExist", "Tài nguyên đã tồn tại trong một buổi học");

    public static Error CourseResourceIsInUse() =>
        new Error("CourseResource.InUse", "Tài nguyên đang được sử dụng trong một khóa học đã xuất bản và không thể cập nhật.");
}
