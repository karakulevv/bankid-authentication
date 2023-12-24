using Application.Cache.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Application.Cache;

public class InMemoryCache : ICache
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;

    public InMemoryCache(IMemoryCache memoryCache, ILogger logger)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task Set<T>(string key, T value, TimeSpan expiration)
    {
        _memoryCache.Set(key, value, expiration);
        _logger.Information("key {key} set for {seconds} sec.", key, expiration.TotalSeconds);
        return Task.Delay(0);
    }

    public Task Remove(string key)
    {
        _memoryCache.Remove(key);
        _logger.Information("key {key} removed", key);
        return Task.Delay(0);
    }

    public Task<T> GetAsync<T>(string key)
    {
        T value = default(T);
        if (_memoryCache.TryGetValue(key, out T obj))
        {
            value = obj;
            _logger.Information("Succesfull retrieved key {key}", key);
        }

        else
        {
            _logger.Information("Value not found for key {key}", key);
        }
        return Task.FromResult(value);
    }
}