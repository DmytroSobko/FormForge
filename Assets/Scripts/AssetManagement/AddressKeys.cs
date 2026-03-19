namespace FormForge.AssetManagement
{
    public static class AddressKeys
    {
        public static class UI
        {
            public static class Screens
            {
                public const string MainMenuScreen = "MainMenuScreen";

                public static class Athletes
                {
                    public const string AthletesScreen = "AthletesScreen";
                    public const string CreateAthleteScreen = "CreateAthleteScreen";
                                    
                    public static class Components
                    {
                        public const string AthleteItemView = "AthleteItemView";
                        public const string AthleteTypeItemView = "AthleteTypeItemView";
                    }
                }
    
                public static class TrainingPlans
                {
                    public const string TrainingPlansScreen = "TrainingPlansScreen";
                    public const string CreateTrainingPlanScreen = "CreateTrainingPlanScreen";
                    
                    public static class Components
                    {
                        public const string TrainingPlanItemView = "ExerciseItemView";
                    }
                }
            }
            
            public static class Tooltips
            {
                public const string StatRow = "TooltipStatRow";
            }
        }
        
        public static class ScriptableObjects
        {
            public static class VisualDatabases
            {
                public const string AthleteTypeVisualsDatabase = "AthleteTypeVisualsDatabase";
                public const string ExerciseVisualsDatabase = "ExerciseVisualsDatabase";
                public const string ToastVisualsDatabase = "ToastVisualsDatabase";
            }
        }
    }
}