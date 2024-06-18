namespace LibraSoft.Core.Interfaces
{
    public interface ICacheService
    {
        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, string tag, DateTime? Expires = null);
        public Task InvalidateCacheAsync(string tag);
    }
}
