using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class CourseVerificationErrors
{
    public static Error VerificationRequestAlreadyExists() =>
        new Error("CourseVerification.RequestAlreadyExists", "Gia sư đã gửi yêu cầu xác minh cho khóa học này");
}