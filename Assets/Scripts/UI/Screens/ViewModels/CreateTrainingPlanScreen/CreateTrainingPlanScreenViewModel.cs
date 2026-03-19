using FormForge.AssetManagement;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using UnityEngine;

namespace FormForge.UI.Screens.ViewModels.CreateTrainingPlanScreen
{
    public class CreateTrainingPlanScreenViewModel: IScreenViewModel
    {
        public static string s_Address = AddressKeys.UI.Screens.TrainingPlans.CreateTrainingPlanScreen;

        public GameObject ExerciseItemPrefab
        {
            get;
        }
        
        public CreateTrainingPlanScreenViewModel(GameObject exerciseItemPrefab)
        {
            ExerciseItemPrefab = exerciseItemPrefab;
        }
    }
}