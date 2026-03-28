using EngConnect.BuildingBlock.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Domain.DomainErrors
{
    public static class TutorErrors
    {
        public static Error TutorNotFound() =>
            new Error("Tutor.NotFound", "Không tìm thấy gia sư.");

        public static Error InvalidStatus(string status) =>
            new Error("Tutor.InvalidStatus", $"Trạng thái gia sư không hợp lệ: '{status}'.");

        public static Error InvalidVerifiedStatus(string status) =>
            new Error("Tutor.InvalidVerifiedStatus", $"Trạng thái xác thực gia sư không hợp lệ: '{status}'.");

        public static Error InvalidHeadline() =>
            new Error("Tutor.InvalidHeadline", "Tiêu đề gia sư không được để trống và không được vượt quá 255 ký tự.");

        public static Error InvalidBio() =>
            new Error("Tutor.InvalidBio", "Mô tả gia sư không được để trống.");
        
        public static Error InvalidUserId() =>
            new Error("Tutor.InvalidUserId", "Người dùng của gia sư không hợp lệ.");

        public static Error VerificationRequestAlreadyPending(Guid tutorId) =>
            new Error("Tutor.VerificationRequestAlreadyPending", $"Gia sư với Id '{tutorId}' đã có yêu cầu xác thực đang chờ xử lý.");

        public static Error InvalidVerificationRequestId() =>
            new Error("Tutor.InvalidVerificationRequestId", "Yêu cầu xác thực gia sư không hợp lệ.");

        public static Error InvalidRejectionReason() =>
            new Error("Tutor.InvalidRejectionReason", "Lý do từ chối không hợp lệ.");

        public static Error VerificationRequestNotFound(Guid requestId) =>
            new Error("Tutor.VerificationRequest.NotFound", $"Không tìm thấy yêu cầu xác thực gia sư với Id '{requestId}'.");

        public static Error VerificationRequestAlreadyReviewed(Guid requestId) =>
            new Error("Tutor.VerificationRequest.AlreadyReviewed", $"Yêu cầu xác thực gia sư với Id '{requestId}' đã được xem xét.");
        public static Error TutorProfileAlreadyExists() =>
            new Error("Tutor.ProfileAlreadyExists", "Tài khoản gia sư đã được đăng kí.");
        
        public static Error TutorProfileIncomplete() =>
            new Error("Tutor.ProfileIncomplete", "Hồ sơ gia sư chưa hoàn chỉnh. Vui lòng cập nhật đầy đủ thông tin hồ sơ trước khi thực hiện hành động này.");
        
        public static Error TutorProfileNotVerified() =>
            new Error("Tutor.ProfileNotVerified", "Hồ sơ gia sư chưa được xác thực. Vui lòng chờ quá trình xác thực hoàn tất trước khi thực hiện hành động này.");
    }
}
