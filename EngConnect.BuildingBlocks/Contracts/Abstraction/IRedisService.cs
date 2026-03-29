namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IRedisService
{
    Task<string?> GetCacheAsync(string key);
    Task<T?> GetCacheAsync<T>(string key);
    Task<bool> SetCacheAsync(string key, object value, TimeSpan? timeToLive, bool isSerialized = true);
    Task<bool> DeleteCacheAsync(string key);
    Task DeleteCacheWithPatternAsync(string pattern);
    Task<IEnumerable<string>> GetKeysByPatternAsync(string pattern);
    //Redis Hash
    Task<bool> HashSetAsync(string key, string field, long value, TimeSpan? timeToLive);
    Task<bool> HashSetAsync(string key, IDictionary<string, long>? values, TimeSpan? timeToLive );
    Task<string?>HashGetAsync(string key, string field);
    Task<bool>HashDeleteAsync(string key, string field);
    Task<Dictionary<string, int>> HashGetAllAsync(string key);
    Task<long> HashIncrementAsync(string key, string field, long value);
    Task<bool> SortedSetAddAsync(string key, string member, double score, TimeSpan? timeToLive = null);
    Task<IEnumerable<string>> SortedSetRangeAsync(string key);
    Task<long> SortedSetLengthAsync(string key);
}