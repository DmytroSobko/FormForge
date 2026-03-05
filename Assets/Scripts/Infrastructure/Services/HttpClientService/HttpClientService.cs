using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        
        private const int MaxRetries = 3;
        private const int RetryDelayMs = 500;
        
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private bool m_DebugHttp = true;
#else
        private bool m_DebugHttp = false;
#endif

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
            var url = BaseApiUrl + endpoint;

            LogRequest("GET", url);

            using var response = await SendWithRetry(() =>
                m_Client.GetAsync(url).AsUniTask()
            );

            var json = await response.Content.ReadAsStringAsync();

            LogResponse(response, json);

            if (!response.IsSuccessStatusCode)
            {
                HandleError(response, json);
            }

            return JsonConvert.JsonConvert.DeserializeObject<T>(json, m_JsonSettings);
        }

        public async UniTask<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
        {
            var url = BaseApiUrl + endpoint;

            var json = JsonConvert.JsonConvert.SerializeObject(payload, m_JsonSettings);

            LogRequest("POST", url, json);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await SendWithRetry(() =>
                m_Client.PostAsync(url, content).AsUniTask()
            );

            var responseJson = await response.Content.ReadAsStringAsync();

            LogResponse(response, responseJson);

            if (!response.IsSuccessStatusCode)
            {
                HandleError(response, responseJson);
            }

            return JsonConvert.JsonConvert.DeserializeObject<TResponse>(responseJson, m_JsonSettings);
        }
        
        private async UniTask<HttpResponseMessage> SendWithRetry(Func<UniTask<HttpResponseMessage>> request)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    return await request();
                }
                catch (HttpRequestException e)
                {
                    m_Logger.LogWarning($"HTTP request failed (attempt {attempt}): {e.Message}");

                    if (attempt == MaxRetries)
                    {
                        throw;
                    }

                    await UniTask.Delay(RetryDelayMs);
                }
                catch (TaskCanceledException e)
                {
                    m_Logger.LogWarning($"HTTP timeout (attempt {attempt}): {e.Message}");

                    if (attempt == MaxRetries)
                    {
                        throw;
                    }

                    await UniTask.Delay(RetryDelayMs);
                }
            }

            throw new Exception("Unexpected HTTP retry failure");
        }
        
        private void HandleError(HttpResponseMessage response, string body)
        {
            try
            {
                var error = JsonConvert.JsonConvert.DeserializeObject<ErrorResponse>(body);

                throw new ApiException((int)response.StatusCode, error?.Error ?? "unknown_error",
                    error?.Message ?? "Unknown server error");
            }
            catch (JsonConvert.JsonException)
            {
                throw new ApiException((int)response.StatusCode, "unknown_error", body);
            }
        }
        
        private void LogRequest(string method, string url, string body = null)
        {
            if (!m_DebugHttp)
            {
                return;
            }

            m_Logger.Log($"HTTP {method} {url}");

            if (!string.IsNullOrEmpty(body))
            {
                m_Logger.Log($"Request Body: {body}");
            }
        }
        
        private void LogResponse(HttpResponseMessage response, string body)
        {
            if (!m_DebugHttp)
            {
                return;
            }

            m_Logger.Log($"HTTP Response {(int)response.StatusCode} {response.StatusCode}");

            if (!string.IsNullOrEmpty(body))
            {
                m_Logger.Log($"Response Body: {body}");
            }
        }
    }
}