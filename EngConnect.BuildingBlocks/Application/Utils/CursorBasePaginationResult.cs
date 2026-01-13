namespace EngConnect.BuildingBlock.Application.Utils;

public class CursorBasePaginationResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
    public string? PreviousCursor { get; set; }
    public string? NextCursor { get; set; }

    public CursorBasePaginationResult(IEnumerable<T> items, bool hasPrevious, bool hasNext,
        string? previousCursor, string? nextCursor)
    {
        Items = items;
        HasPrevious = hasPrevious;
        HasNext = hasNext;
        PreviousCursor = previousCursor;
        NextCursor = nextCursor;
    }
}