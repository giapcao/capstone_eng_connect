using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class OutboxEventErrors
{
    public static Error FailedToGetPendingOrRetryableEvents =>
        new("OutboxEvent.FailedToGetPendingOrRetryableEvents", "Đã xảy ra lỗi khi lấy các OutboxEvent đang chờ hoặc đang thử lại");
}