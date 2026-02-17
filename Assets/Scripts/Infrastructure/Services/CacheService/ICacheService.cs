using System;
using System.Threading.Tasks;

namespace FormForge.Infrastructure.Services.CacheService
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan lifetime);

        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value, TimeSpan lifetime);

        void Update<T>(string key, Func<T, T> updateFunc);

        void Invalidate(string key);

        void Clear();
    }
}