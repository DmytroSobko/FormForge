using FormForge.Infrastructure.UI.Screens.Models;
using UnityEngine;

namespace FormForge.UI.Screens.Models
{
    public class CreateAthleteScreenViewModel : IScreenViewModel
    {
        public static string s_Address = "CreateAthleteScreen";
        
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