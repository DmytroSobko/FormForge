using System;

namespace FormForge.Infrastructure.Services.HttpClientService
{
    [Serializable]
    public class ErrorResponse
    {
        public string Error;
        public string Message;
    }
}