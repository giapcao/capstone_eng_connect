namespace EngConnect.BuildingBlock.Contracts.Shared;

public class RedisPatternBuilder
{
    private readonly List<string> _parts = [];

    public RedisPatternBuilder AddExact(string value)
    {
        _parts.Add(value);
        return this;
    }

    public RedisPatternBuilder AddWildcard()
    {
        _parts.Add("*");
        return this;
    }

    public RedisPatternBuilder AddOptional(string? value)
    {
        _parts.Add(string.IsNullOrEmpty(value) ? "*" : value);
        return this;
    }

    public string Build() => string.Join(":", _parts);
}