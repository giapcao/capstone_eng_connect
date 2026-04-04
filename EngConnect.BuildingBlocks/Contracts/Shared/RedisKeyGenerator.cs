namespace EngConnect.BuildingBlock.Contracts.Shared;

public static class RedisKeyGenerator
{
    private const string EMAIL_VERIFICATION = "EMAIL:Verification";
    private const string EMAIL_RESET_PASSWORD = "EMAIL:ResetPassword";
    private const string CACHE_SETTING_ALL = "SETTING:ALL";
    public const string CACHE_COLLECTION_ALL = "COLLECTION:ALL";
    private const string REFRESH_TOKEN = "AUTH:RefreshToken";
    private const string AdminRefreshToken = "ADMIN:RefreshToken";
    private const string AdminPendingToken = "ADMIN:PendingToken";
    private const string USER_LOGIN_TOKEN = "USER:LoginToken";
    private const string STUDENT_AVATAR = "STUDENT:Avatar";
    private const string TUTOR_AVATAR = "TUTOR:Avatar";
    private const string TUTOR_CV = "TUTOR:CV";
    private const string TUTOR_INTRO_VIDEO = "TUTOR:IntroVideo";
    private const string COURSE_THUMBNAIL = "COURSE:Thumbnail";
    private const string COURSE_DEMO_VIDEO = "COURSE:DemoVideo";

    /// <summary>
    ///     Generate key for pending token
    ///     Pattern: ADMIN:PendingToken:userId
    ///     Value will be the pendingToken
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string GenerateAdminPendingTokenKey(Guid userId) =>
        new RedisPatternBuilder()
            .AddExact(AdminPendingToken)
            .AddExact(userId.ToString())
            .Build();

    /// <summary>
    ///     Generate pattern for all admin pending tokens
    ///     Pattern: ADMIN:PendingToken:*
    /// </summary>
    /// <returns></returns>
    public static string GenerateAdminPendingTokenPattern() =>
        new RedisPatternBuilder()
            .AddExact(AdminPendingToken)
            .AddWildcard()
            .Build();

    /// <summary>
    ///     Generate key for admin refresh token
    ///     Pattern: ADMIN:RefreshToken:userId
    ///     Value will be the refreshToken
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string GenerateAdminRefreshTokenKey(Guid userId) =>
        new RedisPatternBuilder()
            .AddExact(AdminRefreshToken)
            .AddExact(userId.ToString())
            .Build();

    /// <summary>
    ///     Generate pattern for all admin refresh tokens
    ///     Pattern: ADMIN:RefreshToken:*
    /// </summary>
    /// <returns></returns>
    public static string GenerateAdminRefreshTokenPattern() =>
        new RedisPatternBuilder()
            .AddExact(AdminRefreshToken)
            .AddWildcard()
            .Build();

    /// <summary>
    ///     Generate key for email verification token where token is the key
    ///     Pattern: EMAIL:Verification
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static string GenerateEmailVerificationTokenAsKey(string token) =>
        new RedisPatternBuilder()
            .AddExact(EMAIL_VERIFICATION)
            .AddExact(token)
            .Build();

    /// <summary>
    ///     Generate key for email reset password token where token is the key
    ///     Pattern: EMAIL:ResetPassword
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static string GenerateEmailResetPasswordTokenAsKey(string token) =>
        new RedisPatternBuilder()
            .AddExact(EMAIL_RESET_PASSWORD)
            .AddExact(token)
            .Build();

    /// <summary>
    ///     Generate key for refresh token
    ///     Pattern: AUTH:RefreshToken:userId:token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static string GenerateRefreshTokenKey(Guid userId, string token) =>
        new RedisPatternBuilder()
            .AddExact(REFRESH_TOKEN)
            .AddExact(userId.ToString())
            .AddExact(token)
            .Build();

    /// <summary>
    ///     Generate pattern for deleting all refresh tokens of a user
    ///     Pattern: AUTH:RefreshToken:userId:*
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string GenerateRefreshTokenKeyDeletePattern(Guid userId) =>
        new RedisPatternBuilder()
            .AddExact(REFRESH_TOKEN)
            .AddExact(userId.ToString())
            .AddWildcard()
            .Build();

    /// <summary>
    ///     Get cache key for all settings
    ///     Pattern: SETTING:ALL
    /// </summary>
    /// <returns></returns>
    public static string GetCacheAllSettingKey()
    {
        return CACHE_SETTING_ALL;
    }

    /// <summary>
    ///     Get cache key for all collections
    ///     Pattern: COLLECTION:ALL
    /// </summary>
    /// <returns></returns>
    public static string GetCacheAllCollectionKey()
    {
        return CACHE_COLLECTION_ALL;
    }

    public static string GenerateUserLoginTokenKey(string token) =>
        new RedisPatternBuilder()
            .AddExact(USER_LOGIN_TOKEN)
            .AddExact(token)
            .Build();

    public static string GenerateStudentAvatarKey(Guid studentId) =>
        new RedisPatternBuilder()
            .AddExact(STUDENT_AVATAR)
            .AddExact(studentId.ToString())
            .Build();
    
    public static string GenerateTutorAvatarKey(Guid tutorId) =>
        new RedisPatternBuilder()
            .AddExact(TUTOR_AVATAR)
            .AddExact(tutorId.ToString())
            .Build();
    
    public static string GenerateTutorCvKey(Guid tutorId) =>
        new RedisPatternBuilder()
            .AddExact(TUTOR_CV)
            .AddExact(tutorId.ToString())
            .Build();
    
    public static string GenerateTutorIntroVideoKey(Guid tutorId) =>
        new RedisPatternBuilder()
            .AddExact(TUTOR_INTRO_VIDEO)
            .AddExact(tutorId.ToString())
            .Build();
    
    public static string GenerateCourseThumbnailKey(Guid courseId) =>
        new RedisPatternBuilder()
            .AddExact(COURSE_THUMBNAIL)
            .AddExact(courseId.ToString())
            .Build();
    
    public static string GenerateCourseDemoVideoKey(Guid courseId) =>
        new RedisPatternBuilder()
            .AddExact(COURSE_DEMO_VIDEO)
            .AddExact(courseId.ToString())
            .Build();
}