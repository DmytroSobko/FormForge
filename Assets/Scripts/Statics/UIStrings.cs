namespace FormForge.Statics
{
    public static class UIStrings
    {
        public static class Athletes
        {
            public const string NoAthletesCreatedYet = "No athletes have been created yet.";
        }
        
        public static class CreateAthlete
        {
            public static class Validation
            {
                public const string NameEmpty = "Name cannot be empty.";
                public const string NameTooShort = "Name must be at least {0} characters.";
                public const string NameTooLong = "Name cannot exceed {0} characters.";
                public const string NameInvalidCharacters = "Name contains invalid characters.";
            }
            
            public static class Toast
            {
                public const string Error = "Failed to create athlete.";
                
                public static string Success(string name)
                    => $"Athlete '{name}' created";
            }

            public const string SelectAthleteType = "Please select an athlete type.";
            public const string Creating = "Creating an athlete...";
            public const string FailedWithError = "Failed to create athlete.\nError {0}: {1}";
        }

        public static class TrainingPlans
        {
            public const string NoTrainingPlansCreatedYet = "No training plans have been created yet.";
        }
        
        public static class CreateTrainingPlan
        {
            public static class Validation
            {
                public const string NameEmpty = "Name cannot be empty.";
                public const string NameTooShort = "Name must be at least {0} characters.";
                public const string NameTooLong = "Name cannot exceed {0} characters.";
                public const string NameInvalidCharacters = "Name contains invalid characters.";
            }
            
            public static class Toast
            {
                public const string Error = "Failed to create training plan.";
                
                public static string Success(string name)
                    => $"Training plan '{name}' created";
            }

            public const string Creating = "Creating a training plan...";
            public const string FailedWithError = "Failed to create training plan.\nError {0}: {1}";
        }
    }
}