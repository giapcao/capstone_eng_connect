using Microsoft.AspNetCore.Mvc;

namespace EngConnect.BuildingBlock.Application.Params;

public class CursorBaseParams
{
    private string? _after;
    private string? _before;
    private int _limit = 10;

    [FromQuery(Name = "limit")]
    public int Limit
    {
        get => _limit;
        set => _limit = value < 1 ? 1 : value;
    }

    [FromQuery(Name = "after")]
    public string? After
    {
        get => _after;
        set => _after = string.IsNullOrEmpty(value) ? null : value;
    }

    [FromQuery(Name = "before")]
    public string? Before
    {
        get => _before;
        set => _before = string.IsNullOrEmpty(value) ? null : value;
    }
}