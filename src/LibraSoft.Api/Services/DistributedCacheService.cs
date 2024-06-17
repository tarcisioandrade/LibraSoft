using System.Text.Json;
using LibraSoft.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace LibraSoft.Api.Services
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IMemoryCache _tagCache;
        public static DistributedCacheEntryOptions DefaultExpiration => new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
        };

        public DistributedCacheService(IDistributedCache cache, IMemoryCache tagCache)
        {
            _cache = cache;
            _tagCache = tagCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, DateTime? Expires = null, string? tag = null)
        {
            var cachedData = await _cache.GetStringAsync(key);

            if (cachedData is not null)
            {
                return JsonSerializer.Deserialize<T>(cachedData)!;
            }

            var data = await factory();

            var cacheOptions = Expires is null ? DefaultExpiration : new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = Expires,
            };

            if(!string.IsNullOrEmpty(tag))
            {
#pragma warning disable CS8600
                if (!_tagCache.TryGetValue(tag, out HashSet<string> keys))
                {
                    keys = [];
                    _tagCache.Set(tag, keys);
                }
                keys!.Add(key);
            }
                
            await _cache.SetStringAsync(
                key,    
                JsonSerializer.Serialize(data),
                cacheOptions);

            return data;    
        }

        public async Task InvalidateCacheAsync(string tag)
        {
            if (_tagCache.TryGetValue(tag, out HashSet<string> keys))
            {
#pragma warning disable CS8602
                foreach (var key in keys)
                {
                    await _cache.RemoveAsync(key);
                }
                _tagCache.Remove(tag);
            }
        }
    }
}
