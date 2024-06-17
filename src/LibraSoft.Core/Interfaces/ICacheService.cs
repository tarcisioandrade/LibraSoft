namespace LibraSoft.Core.Interfaces
{
    public interface ICacheService
    {
        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, DateTime? Expires = null, string? tag = null);
        public Task InvalidateCacheAsync(string tag);
    }
}
