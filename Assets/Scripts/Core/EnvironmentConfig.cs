namespace FormForge.Core
{
    public static class EnvironmentConfig
    {
#if UNITY_EDITOR
        public const string ApiBaseUrl = "http://localhost:8080/api";
#else
        public const string ApiBaseUrl = "https://api.formforge.com";
#endif
    }
}