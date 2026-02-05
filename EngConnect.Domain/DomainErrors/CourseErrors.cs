using EngConnect.BuildingBlock.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Domain.DomainErrors
{
    public static class CourseErrors
    {
        public static Error InvalidCourseId() =>
            new Error("Course.InvalidCourseId", "Khóa học không hợp lệ.");

        public static Error VerificationRequestAlreadyPending(Guid courseId) =>
            new Error("Course.VerificationRequestAlreadyPending", $"Khóa học với Id '{courseId}' đã có yêu cầu xác thực đang chờ xử lý.");

        public static Error NotFound(Guid courseId) =>
            new Error("Course.NotFound", $"Không tìm thấy khóa học với Id '{courseId}'.");

        public static Error InvalidVerificationRequestId() =>
            new Error("Course.InvalidVerificationRequestId", "Yêu cầu xác thực khóa học không hợp lệ.");

        public static Error InvalidRejectionReason() =>
            new Error("Course.InvalidRejectionReason", "Lý do từ chối không hợp lệ.");

        public static Error VerificationRequestNotFound(Guid requestId) =>
            new Error("Course.VerificationRequest.NotFound", $"Không tìm thấy yêu cầu xác thực khóa học với Id '{requestId}'.");

        public static Error VerificationRequestAlreadyReviewed(Guid requestId) =>
            new Error("Course.VerificationRequest.AlreadyReviewed", $"Yêu cầu xác thực khóa học với Id '{requestId}' đã được xem xét.");
    }
}
