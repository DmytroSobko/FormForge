using System.Collections.Generic;
using FormForge.Infrastructure.UI.Screens.Models;
using FormForge.Runtime.Models.Athletes;
using UnityEngine;

namespace FormForge.UI.Screens.Models.AthletesScreen
{
    public class AthletesScreenViewModel : IScreenViewModel
    {
        public static string s_Address = "AthletesScreen";

        public IReadOnlyList<Athlete> Athletes
        {
            get;
        }

        public GameObject ItemPrefab
        {
            get;
        }

        public AthletesScreenViewModel(IReadOnlyList<Athlete> athletes, GameObject itemPrefab)
        {
            Athletes = athletes;
            ItemPrefab = itemPrefab;
        }
    }
}