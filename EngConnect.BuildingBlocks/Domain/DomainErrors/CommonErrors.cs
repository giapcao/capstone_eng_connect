using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Domain.DomainErrors;

public static class CommonErrors
{
    /// <summary>
    ///     Returns a not found error for the specified entity
    ///     Message will be in format: "Không tìm thấy {message}"
    /// </summary>
    /// <param name="message">The name of the entity</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Error NotFound<T>(string message) where T : class
    {
        return new Error($" {typeof(T).Name}.NotFound", $"Không tìm thấy {message}");
    }

    /// <summary>
    ///     Returns an unauthorized error for the specified entity
    ///     Message will be in format: "Người dùng không có quyền truy cập vào tài nguyên"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Error Unauthorized<T>() where T : class
    {
        return new Error($"{typeof(T).Name}.Unauthorized", "Người dùng không có quyền truy cập vào tài nguyên");
    }

    /// <summary>
    ///     Returns a validation failed error
    ///     Message will be in format: "Dữ liệu không hợp lệ: {message}"
    /// </summary>
    /// <param name="message">The error message</param>
    /// <returns></returns>
    public static Error ValidationFailed(string message)
    {
        return new Error("Validation.Failed", $"Dữ liệu không hợp lệ: {message}");
    }

    public static Error ValidationFailed<T>(string message) where T : class
    {
        return new Error($"{typeof(T).Name}.Validation.Failed", $"Dữ liệu không hợp lệ: {message}");
    }

    /// <summary>
    ///     Returns an already existed error for the specified attribute
    ///     Message will be in format: "{message} đã tồn tại"
    /// </summary>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="message">The name of the attribute in message</param>
    /// <returns></returns>
    public static Error AlreadyExists(string attributeName, string message)
    {
        return new Error($"{attributeName}.AlreadyExists", $"{message} đã tồn tại");
    }

    /// <summary>
    ///     Returns an invalid operation error
    ///     Message will be in format: "{message}"
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Error InvalidOperation(string message)
    {
        return new Error("InvalidOperation", message);
    }

    /// <summary>
    ///     Returns an internal server error with the specified message.
    ///     Message will indicate a generic server-side error has occurred.
    /// </summary>
    /// <param name="message">The error message describing the issue.</param>
    /// <returns>A structured error object representing the internal server error.</returns>
    public static Error InternalServerError(string message)
    {
        return new Error("Server.Error", message);
    }
    
    public static Error InternalServerError()  => new("Server.Error", "Có lỗi xảy ra trong quá trình xử lý yêu cầu. Vui lòng thử lại sau.");
}