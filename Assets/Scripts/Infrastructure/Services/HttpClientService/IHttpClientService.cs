using Cysharp.Threading.Tasks;

namespace FormForge.Infrastructure.Services.HttpClientService
{
    public interface IHttpClientService
    {
        public string BaseApiUrl { get; }
        
        void SetBaseApiUrl(string url);
        UniTask<T> GetAsync<T>(string endpoint);
        UniTask<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload);
    }
}