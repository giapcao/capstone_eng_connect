namespace EngConnect.BuildingBlock.Application.Params;

public class SearchParams
{
    private string _searchTerm = "";

    // If the value is null or empty, set it to an empty string
    public string SearchTerm
    {
        get => _searchTerm;
        set => _searchTerm = string.IsNullOrEmpty(value) ? "" : value;
    }

    // Return a normalized version of the search term (all lowercase and without spaces) for searching purposes
    public string NormalizedSearchTerm => SearchTerm.Replace(" ", "").Trim().ToLower();
}