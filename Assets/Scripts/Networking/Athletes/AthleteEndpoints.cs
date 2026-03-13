namespace FormForge.Networking.Athletes
{
    public static class AthleteEndpoints
    {
        public const string Base = "/athletes";

        public static string Paginated(int limit, int offset)
            => $"{Base}?limit={limit}&offset={offset}";

        public static string ById(string id)
            => $"{Base}/{id}";
    }
}