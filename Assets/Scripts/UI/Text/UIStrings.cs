namespace FormForge.UI.Text
{
    public static class UIStrings
    {
        public static class CreateAthlete
        {
            public static class Validation
            {
                public const string NameEmpty = "Name cannot be empty.";
                public const string NameTooShort = "Name must be at least {0} characters.";
                public const string NameTooLong = "Name cannot exceed {0} characters.";
                public const string NameInvalidCharacters = "Name contains invalid characters.";
            }
            
            public const string SelectAthleteType = "Please select an athlete type.";
            public const string Creating = "Creating an athlete...";
            public const string Failed = "Failed to create athlete.";
            public const string FailedWithError = "Failed to create athlete.\nError {0}: {1}";
        }
    }
}