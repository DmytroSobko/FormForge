using FormForge.Infrastructure.UI.Screens.ViewModels;
using FormForge.UI.Screens.Pagination.DataProviders;
using UnityEngine;

namespace FormForge.UI.Screens.ViewModels.AthletesScreen
{
    public class AthletesScreenViewModel : IScreenViewModel
    {
        public static string s_Address = "AthletesScreen";
        
        public GameObject ItemPrefab
        {
            get;
        }
        
        public AthletesPaginatedDataProvider PaginatedDataProvider
        {
            get;
        }

        public AthletesScreenViewModel(AthletesPaginatedDataProvider paginatedDataProvider, 
            GameObject itemPrefab)
        {
            ItemPrefab = itemPrefab;
            PaginatedDataProvider = paginatedDataProvider;
        }
    }
}