using System;
using Cysharp.Threading.Tasks;

namespace FormForge.Infrastructure.Services.CacheService
{
    public interface ICacheService
    {
        UniTask<T> GetOrCreateAsync<T>(string key, Func<UniTask<T>> factory, TimeSpan lifetime);

        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value, TimeSpan lifetime);

        void Update<T>(string key, Func<T, T> updateFunc);

        void Invalidate(string key);

        void Clear();
    }
}