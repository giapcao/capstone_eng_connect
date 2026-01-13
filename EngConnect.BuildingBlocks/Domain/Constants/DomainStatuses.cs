namespace EngConnect.BuildingBlock.Domain.Constants;

/// <summary>
///     This is a list of common statuses that is shared across all domains
///     This ensures consistent status values (for filtering and visibility)
/// </summary>
public static class DomainStatuses
{
    public const int Active = 1;
    public const int Inactive = 2;
    public const int Draft = 3;
    public const int Expired = 4;
    public const int Suspended = 5;

    public const int Reserved = 6;
    public const int Pending = 7;
    public const int Paid = 8;
    public const int Cancelled = 9;
    
    public const int Processing = 10;
    public const int Closed = 11;
    
    public static int[] All => [Active, Inactive, Draft, Expired, Suspended, Reserved, Pending, Paid, Cancelled, Processing, Closed];
}