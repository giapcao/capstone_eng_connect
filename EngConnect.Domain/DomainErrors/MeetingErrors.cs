using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class MeetingErrors
{
    public static Error LessonNotFound(Guid lessonId) =>
        new Error("Meeting.LessonNotFound", $"Không tìm thấy buổi học với Id '{lessonId}'.");

    public static Error NotAuthorized() =>
        new Error("Meeting.NotAuthorized", "Người dùng không có quyền tham gia buổi học này.");

    public static Error RoomAlreadyCreated(Guid lessonId) =>
        new Error("Meeting.RoomAlreadyCreated", $"Phòng học cho buổi học '{lessonId}' đã được tạo.");

    public static Error RoomNotFound(Guid lessonId) =>
        new Error("Meeting.RoomNotFound", $"Không tìm thấy phòng học cho buổi học '{lessonId}'.");

    public static Error MeetingAlreadyEnded(Guid lessonId) =>
        new Error("Meeting.AlreadyEnded", $"Buổi học '{lessonId}' đã kết thúc.");

    public static Error UserAlreadyInRoom() =>
        new Error("Meeting.UserAlreadyInRoom", "Người dùng đã ở trong phòng học.");

    public static Error InvalidLessonId() =>
        new Error("Meeting.InvalidLessonId", "Id buổi học không hợp lệ.");
}