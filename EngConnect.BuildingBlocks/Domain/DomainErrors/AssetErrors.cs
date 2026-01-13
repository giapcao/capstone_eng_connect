using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Domain.DomainErrors;

public class AssetErrors
{
    // public static Error NotFound<T>() where T : class
    // {
    //     return new Error($"Asset.{typeof(T).Name}.NotFound", "Không tìm thấy tài nguyên");
    // }

    public static Error Unauthorized<T>() where T : class
    {
        return new Error($"Asset.{typeof(T).Name}.Unauthorized", "Người dùng không có quyền truy cập vào tài nguyên");
    }
    //
    // public static Error InvalidId<T>() where T : class
    // {
    //     return new Error($"Asset.{typeof(T).Name}.InvalidId", "ID tài nguyên không hợp lệ");
    // }

    public static Error Unauthorized()
    {
        return new Error("Asset.Unauthorized", "Người dùng không có quyền truy cập vào tài nguyên");
    }
}