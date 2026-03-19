using System.Text.RegularExpressions;
using FormForge.Statics;

namespace FormForge.Helpers
{
    public static class ValidationHelper
    {
        private static readonly string s_NamePattern = @"^[\p{L}\p{N} _-]+$";
        public static string ValidateAthleteName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return UIStrings.CreateAthlete.Validation.NameEmpty;
            }

            name = name.Trim();

            if (name.Length < Constants.MinNameLength)
            {
                return string.Format(UIStrings.CreateAthlete.Validation.NameTooShort, 
                    Constants.MinNameLength);
            }

            if (name.Length > Constants.MaxNameLength)
            {
                return string.Format(UIStrings.CreateAthlete.Validation.NameTooLong,
                    Constants.MaxNameLength);
            }

            if (!Regex.IsMatch(name, s_NamePattern))
            {
                return UIStrings.CreateAthlete.Validation.NameInvalidCharacters;
            }

            return null;
        }
        
        public static string ValidateTrainingPlanName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return UIStrings.CreateAthlete.Validation.NameEmpty;
            }

            name = name.Trim();

            if (name.Length < Constants.MinNameLength)
            {
                return string.Format(UIStrings.CreateAthlete.Validation.NameTooShort, 
                    Constants.MinNameLength);
            }

            if (name.Length > Constants.MaxNameLength)
            {
                return string.Format(UIStrings.CreateAthlete.Validation.NameTooLong,
                    Constants.MaxNameLength);
            }

            if (!Regex.IsMatch(name, s_NamePattern))
            {
                return UIStrings.CreateAthlete.Validation.NameInvalidCharacters;
            }

            return null;
        }
    }
}