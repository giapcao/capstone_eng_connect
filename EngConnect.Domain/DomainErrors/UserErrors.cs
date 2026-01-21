using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class UserErrors
{
    public static Error UserNotFound() =>
         new Error("User.NotFound", "Không tìm thấy người dùng.");
    

    public static Error InvalidPassword() =>
        new Error("User.InvalidPassword", "Mật khẩu không hợp lệ.");


    public static Error PhoneAlreadyExist() =>
        new Error("User.PhoneAlreadyExist", "Số điện thoại đã được sử dụng.");
}