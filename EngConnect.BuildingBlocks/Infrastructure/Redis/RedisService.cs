using System.Text.Json;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using StackExchange.Redis;

namespace EngConnect.BuildingBlock.Infrastructure.Redis;

public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private readonly IDatabase _redis = redis.GetDatabase();

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<string?> GetCacheAsync(string key)
    {
        var data = await _redis.StringGetAsync(key);
        return data.IsNullOrEmpty ? null : data.ToString();
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var data = await GetCacheAsync(key);
        return data == null ? default : JsonSerializer.Deserialize<T>(data, _jsonSerializerOptions);
    }

    public async Task<bool> SetCacheAsync(string key, object value, TimeSpan? timeToLive, bool isSerialized = true)
    {
        if (!isSerialized)
        {
            return await _redis.StringSetAsync(key, value.ToString(), timeToLive);
        }

        var data = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        return await _redis.StringSetAsync(key, data, timeToLive);
    }

    public Task<bool> DeleteCacheAsync(string key)
    {
        return _redis.KeyDeleteAsync(key);
    }

    public Task DeleteCacheWithPatternAsync(string pattern)
    {
        var server = _redis.Multiplexer.GetServer(_redis.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(0, pattern).ToArray();
        return _redis.KeyDeleteAsync(keys);
    }

    public Task<IEnumerable<string>> GetKeysByPatternAsync(string pattern)
    {
        var server = _redis.Multiplexer.GetServer(_redis.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(0, pattern).Select(k => k.ToString());
        return Task.FromResult(keys);
    }

    
    public async Task<bool> HashSetAsync(string key, string field, long value, TimeSpan? timeToLive)
    {
        var result = await _redis.HashSetAsync(key, field, value);
        if (timeToLive != TimeSpan.Zero)
        {
            await _redis.KeyExpireAsync(key, timeToLive);
        }

        return result;
    }

    public async Task<bool> HashSetAsync(
        string key, 
        IDictionary<string, long>? values, 
        TimeSpan? timeToLive)
    {
        if (values == null || values.Count == 0)
        {
            return false;
        }
        //Convert to HashEntry List
        var hashEntries = new List<HashEntry>(values.Count);
        foreach (var kvp in values)
        {
            hashEntries.Add(new HashEntry(kvp.Key, kvp.Value));
        }
        
        await _redis.HashSetAsync(key, hashEntries.ToArray());
        if (timeToLive.HasValue)
        {
            await _redis.KeyExpireAsync(key, timeToLive);
        }
        return true;
    }

    public async Task<string?> HashGetAsync(string key, string field)
    {
        var data = await  _redis.HashGetAsync(key, field);
        return data.IsNullOrEmpty ? null : data.ToString();
    }


    public Task<bool> HashDeleteAsync(string key, string field)
    {
        return _redis.HashDeleteAsync(key, field);
    }
    

    public async Task<Dictionary<string, int>> HashGetAllAsync(string key)
    {
        var entries = await _redis.HashGetAllAsync(key);
        var result = new Dictionary<string, int>();
        foreach (var entry in entries)
        {
            result[entry.Name!] = (int)entry.Value;
        }
        return result;
    }
    
    public async Task<long> HashIncrementAsync(string key, string field, long value)
    {
        return await _redis.HashIncrementAsync(key, field, value);
    }
}