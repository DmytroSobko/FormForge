namespace FormForge.Helpers
{
    public static class ValidationHelper
    {
        public static string ValidateAthleteName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "Name cannot be empty.";
            }

            name = name.Trim();

            if (name.Length < Constants.MinNameLength)
            {
                return $"Name must be at least {Constants.MinNameLength} characters.";
            }

            if (name.Length > Constants.MaxNameLength)
            {
                return $"Name cannot exceed {Constants.MaxNameLength} characters.";
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[\p{L}\p{N} _-]+$"))
            {
                return "Name contains invalid characters.";
            }

            return null;
        }
    }
}