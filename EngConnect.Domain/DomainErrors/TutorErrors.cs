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

        public static Error InvalidYearsExperience() =>
            new Error("Tutor.InvalidYearsExperience", "Số năm kinh nghiệm không hợp lệ.");

        public static Error InvalidSlotsCount() =>
            new Error("Tutor.InvalidSlotsCount", "Số lượng slot không hợp lệ.");

        public static Error InvalidUserId() =>
            new Error("Tutor.InvalidUserId", "Người dùng của gia sư không hợp lệ.");
    }
}
