using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class UserErrors
{
    public static Error UserNotFound() =>
         new Error("User.NotFound", "Không tìm thấy người dùng.");
    
    public static Error InvalidPassword() =>
        new Error("User.InvalidPassword", "Mật khẩu không hợp lệ.");
    
    public static Error UserAlreadyExists() =>
        new Error("User.AlreadyExists", "Người dùng đã tồn tại.");
    
    public static Error PhoneAlreadyExist() =>
        new Error("User.PhoneAlreadyExist", "Số điện thoại đã được sử dụng.");
    
    public static Error InvalidOrExpiredPasswordResetToken() =>
        new Error("User.InvalidOrExpiredPasswordResetToken", "Mã đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");
    
    public static Error InvalidUserRole() =>
        new Error("User.InvalidUserRole", "Vai trò người dùng không hợp lệ.");
}