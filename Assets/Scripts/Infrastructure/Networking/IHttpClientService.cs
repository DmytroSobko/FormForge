using System.Threading.Tasks;

namespace FormForge.Infrastructure.Networking
{
    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string url);
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest payload);
    }
}