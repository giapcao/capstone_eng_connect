using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class AuthErrors
{
    public static Error InvalidOrExpiredEmailVerificationToken () =>
        new Error("Auth.InvalidOrExpiredEmailVerificationToken",
            "Mã xác thực email không hợp lệ hoặc đã hết hạn.");
    public static Error ErrorInWhileRefreshToken() =>
        new Error("Auth.ErrorInWhileRefreshToken",
            "Đã xảy ra lỗi trong quá trình làm mới token.");

    public static class RefreshToken
    {
        public static Error InvalidToken() =>
            new Error("Auth.RefreshToken.InvalidToken", "Token không hợp lệ.");

        public static Error ExpiredToken() =>
            new Error("Auth.RefreshToken.ExpiredToken", "Token đã hết hạn.");
    }
    
    public static class Logout
    {
        public static Error InvalidToken() =>
            new Error("Auth.Logout.InvalidToken", "Token không hợp lệ.");
    }
}