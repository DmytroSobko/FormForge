using FormForge.AssetManagement;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using UnityEngine;

namespace FormForge.UI.Screens.ViewModels.CreateAthleteScreen
{
    public class CreateAthleteScreenViewModel : IScreenViewModel
    {
        public static string s_Address = AddressKeys.UI.Screens.CreateAthleteScreen;
        
        public GameObject ItemPrefab
        {
            get;
        }

        public CreateAthleteScreenViewModel(GameObject itemPrefab)
        {
            ItemPrefab = itemPrefab;
        }
    }
}