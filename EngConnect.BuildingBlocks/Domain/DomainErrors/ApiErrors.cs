using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Domain.DomainErrors;

public static class ApiErrors
{
    public static Error ApiCallFailed => new("ApiService.ApiCallFailed", "Gọi API thất bại");

    public static class ViettelPostErrors
    {
        public static Error InvalidResponse => new("ApiService.ViettelPost", "Dữ liệu trả về từ Viettel Post không hợp lệ");
    }
}