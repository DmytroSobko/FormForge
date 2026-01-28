using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JsonConvert = Newtonsoft.Json;

namespace FormForge.Infrastructure.Networking
{
    public sealed class HttpClientService : IHttpClientService
    {
        private static readonly HttpClient m_Client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private readonly JsonConvert.JsonSerializerSettings m_JsonSettings =
            new JsonConvert.JsonSerializerSettings
            {
                MissingMemberHandling = JsonConvert.MissingMemberHandling.Ignore,
                NullValueHandling = JsonConvert.NullValueHandling.Ignore
            };

        public async Task<T> GetAsync<T>(string url)
        {
            using var response = await m_Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.JsonConvert.DeserializeObject<T>(json, m_JsonSettings);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest payload)
        {
            var json = JsonConvert.JsonConvert.SerializeObject(payload, m_JsonSettings);

            using var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            using var response = await m_Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.JsonConvert.DeserializeObject<TResponse>(
                responseJson,
                m_JsonSettings
            );
        }
    }
}