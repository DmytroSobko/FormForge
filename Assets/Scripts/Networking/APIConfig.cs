namespace FormForge.Networking
{
    public static class APIConfig
    {
#if UNITY_EDITOR
        private const string ApiVersion = "v1";
        public const string BaseUrl = "http://localhost:8080/api/" + ApiVersion;
#else
        public const string BaseUrl = "https://api.formforge.com";
#endif
    }
}