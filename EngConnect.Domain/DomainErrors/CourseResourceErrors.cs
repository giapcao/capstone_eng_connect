using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class CourseResourceErrors
{
    public static Error RelationshipExist() =>
        new Error("CourseResource.RelationshipExist", "Tài nguyên đã tồn tại trong một buổi học");
}