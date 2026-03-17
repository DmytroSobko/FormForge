using FormForge.AssetManagement;
using FormForge.Infrastructure.UI.Pagination;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using UnityEngine;

namespace FormForge.UI.Screens.ViewModels.TrainingPlansScreen
{
    public class TrainingPlansScreenViewModel : IScreenViewModel
    {
        public static string s_Address = AddressKeys.UI.Screens.TrainingPlans.TrainingPlansScreen;
        
        public GameObject ItemPrefab
        {
            get;
        }
        
        public PaginatedDataProvider<TrainingPlanItemViewModel> PaginatedDataProvider
        {
            get;
        }

        public TrainingPlansScreenViewModel(PaginatedDataProvider<TrainingPlanItemViewModel> paginatedDataProvider, 
            GameObject itemPrefab)
        {
            ItemPrefab = itemPrefab;
            PaginatedDataProvider = paginatedDataProvider;
        }
    }
}