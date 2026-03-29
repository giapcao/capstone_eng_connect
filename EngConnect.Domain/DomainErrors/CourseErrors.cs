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

        public static Error VerificationRequestAlreadyPending () =>
            new Error("Course.VerificationRequestAlreadyPending", $"Khóa học đã có yêu cầu xác thực đang chờ xử lý.");

        public static Error CourseNotFound() =>
            new Error("Course.NotFound", $"Không tìm thấy khóa học.");

        public static Error InvalidVerificationRequestId() =>
            new Error("Course.InvalidVerificationRequestId", "Yêu cầu xác thực khóa học không hợp lệ.");

        public static Error InvalidRejectionReason() =>
            new Error("Course.InvalidRejectionReason", "Lý do từ chối không hợp lệ.");

        public static Error VerificationRequestNotFound() =>
            new Error("Course.VerificationRequest.NotFound", $"Không tìm thấy yêu cầu xác thực khóa học.");

        public static Error VerificationRequestAlreadyReviewed() =>
            new Error("Course.VerificationRequest.AlreadyReviewed", $"Yêu cầu xác thực khóa học đã được xem xét.");
        
        public static Error CourseModuleNotFound() =>
            new Error("CourseModule.NotFound", $"Khóa học phải có ít nhất một module.");
        
        public static Error CategoryNotFound() =>
            new Error("Category.NotFound", $"Một hoặc nhiều danh mục không tồn tại.");
        
        public static Error TutorIsNotOwner() =>
            new Error("Course.TutorIsNotOwner", $"Giáo viên không phải là chủ sở hữu của khóa học.");
        
        public static Error PublishedCourseCannotBeUpdated() =>
            new Error("Course.PublishedCourseCannotBeUpdated", $"Khóa học không thể cập nhật khi đã được xuất bản.");
        
    }
}
