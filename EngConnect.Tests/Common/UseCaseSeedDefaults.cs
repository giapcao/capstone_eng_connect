using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Tests.Common;

internal static class UseCaseSeedDefaults
{
    public static readonly Guid KnownGuid = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly DateTime KnownUtcDateTime = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static readonly DateTimeOffset KnownUtcDateTimeOffset = new(KnownUtcDateTime);
    public const string KnownString = "seed-value";
    public const string KnownEmail = "tester@example.com";
    public const string KnownPassword = "P@ssw0rd!";
    public const string KnownPhone = "0123456789";
    public const string KnownCode = "TEST-CODE";
    public const string KnownStatus = "Active";
    public const string KnownUrl = "https://example.com/test";
    public const string KnownAvatar = "avatars/test-avatar.png";
    public const string KnownFileName = "test.txt";
    public const string KnownContentType = "text/plain";

    public static string KnownPasswordHash => HashHelper.HashPassword(KnownPassword);
}
