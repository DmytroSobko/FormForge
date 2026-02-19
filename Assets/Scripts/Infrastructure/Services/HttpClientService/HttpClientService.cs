using System;
using System.Net.Http;
using System.Text;
using Cysharp.Threading.Tasks;
using FormForge.Core;
using FormForge.Infrastructure.Logging;
using FormForge.Infrastructure.Services.Enums;
using UnityEngine;
using ILogger = FormForge.Infrastructure.Logging.ILogger;
using JsonConvert = Newtonsoft.Json;

namespace FormForge.Infrastructure.Services.HttpClientService
{
    public sealed class HttpClientService : IHttpClientService
    {
        public string BaseApiUrl { get; private set; }

        private static readonly HttpClient m_Client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        
        private ILogger m_Logger = new UnityLogger(nameof(HttpClientService));

        private readonly JsonConvert.JsonSerializerSettings m_JsonSettings =
            new JsonConvert.JsonSerializerSettings
            {
                MissingMemberHandling = JsonConvert.MissingMemberHandling.Ignore,
                NullValueHandling = JsonConvert.NullValueHandling.Ignore
            };
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void RegisterSelf()
        {
            ServiceLocator.RegisterService<IHttpClientService, HttpClientService>(ServiceLifespan.LazySingleton);
        }
        
        public void SetBaseApiUrl(string url)
        {
            m_Logger?.Log($"SetBaseApiUrl {url}");

            BaseApiUrl = url;
        }

        public async UniTask<T> GetAsync<T>(string endpoint)
        {
            using var response = await m_Client.GetAsync(BaseApiUrl + endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.JsonConvert.DeserializeObject<T>(json, m_JsonSettings);
        }

        public async UniTask<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
        {
            var json = JsonConvert.JsonConvert.SerializeObject(payload, m_JsonSettings);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await m_Client.PostAsync(BaseApiUrl + endpoint, content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.JsonConvert.DeserializeObject<TResponse>(
                responseJson,
                m_JsonSettings
            );
        }
    }
}