using EngConnect.BuildingBlock.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Domain.DomainErrors
{
    public static class ScheduleErrors
    {
        public static Error TutorScheduleNotFound() =>
            new Error("Schedule.TutorSchedule.NotFound", "Không tìm thấy lịch rảnh của gia sư.");

        public static Error EnrollmentNotFound() =>
            new Error("Schedule.Enrollment.NotFound", "Không tìm thấy đăng ký khóa học.");

        public static Error TutorNotFound() =>
            new Error("Schedule.Tutor.NotFound", "Không tìm thấy gia sư.");

        public static Error StudentNotFound() =>
            new Error("Schedule.Student.NotFound", "Không tìm thấy học sinh.");

        public static Error LessonNotFound() =>
            new Error("Schedule.Lesson.NotFound", "Không tìm thấy buổi học.");

        public static Error EnrollmentSlotNotFound() =>
            new Error("Schedule.EnrollmentSlot.NotFound", "Không tìm thấy slot đã khóa.");

        public static Error SlotAlreadyLocked() =>
            new Error("Schedule.EnrollmentSlot.AlreadyLocked", "Slot này đã được khóa. Vui lòng chọn slot khác.");

        public static Error SlotNotInTutorTemplate() =>
            new Error("Schedule.EnrollmentSlot.NotInTutorTemplate", "Slot không tồn tại trong lịch rảnh của gia sư.");

        public static Error InvalidWeekday() =>
            new Error("Schedule.InvalidWeekday", "Weekday không hợp lệ (0-6).");

        public static Error InvalidTimeRange() =>
            new Error("Schedule.InvalidTimeRange", "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc.");

        public static Error InvalidRescheduleStatus(string status) =>
            new Error("Schedule.Reschedule.InvalidStatus", $"Trạng thái yêu cầu dời lịch không hợp lệ: '{status}'.");

        public static Error RescheduleRequestNotFound() =>
            new Error("Schedule.Reschedule.NotFound", "Không tìm thấy yêu cầu dời lịch.");

        public static Error RescheduleRequestAlreadyFinalized() =>
            new Error("Schedule.Reschedule.AlreadyFinalized", "Yêu cầu dời lịch đã được xử lý trước đó.");

        public static Error PendingRescheduleRequestAlreadyExists() =>
            new Error("Schedule.Reschedule.PendingAlreadyExists", "Buổi học này đã có yêu cầu dời lịch đang chờ xử lý.");

        public static Error LessonTimeConflict() =>
            new Error("Schedule.TutorSchedule.Conflict", "Lịch rảnh của gia sư bị trùng lặp với lịch đã tồn tại.");

        public static Error ProposedTimeMustBeInFuture() =>
        new Error("Schedule.Lesson.ProposedTimeMustBeInFuture", "Thời gian đề xuất phải ở tương lai.");

        public static Error ProposedTimeMustHaveOneHourBuffer() =>
            new Error("Schedule.Reschedule.OneHourBufferRequired",
                "Thời gian đề xuất phải cách buổi học liền trước và liền sau ít nhất 1 giờ.");
    }
}
