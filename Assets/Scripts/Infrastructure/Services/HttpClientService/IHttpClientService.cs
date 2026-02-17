using System.Threading.Tasks;

namespace FormForge.Infrastructure.Services.HttpClientService
{
    public interface IHttpClientService
    {
        public string BaseApiUrl { get; }
        
        void SetBaseApiUrl(string url);
        Task<T> GetAsync<T>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload);
    }
}