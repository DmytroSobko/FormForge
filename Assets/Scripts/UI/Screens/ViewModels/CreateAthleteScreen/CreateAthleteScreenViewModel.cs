using System.Collections.Generic;
using FormForge.AssetManagement;
using FormForge.Domain.Athletes;
using FormForge.Infrastructure.UI.Screens.ViewModels;
using FormForge.ScriptableObjects.Athletes;
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
        
        public IReadOnlyDictionary<EAthleteType, AthleteTypeConfig> AthleteTypes
        {
            get;
        }
        
        public AthleteTypeVisualsDatabase AthleteTypeVisualsDatabase
        {
            get;
        }
        
        public CreateAthleteScreenViewModel(GameObject itemPrefab,
            IReadOnlyDictionary<EAthleteType, AthleteTypeConfig> athleteTypes,
            AthleteTypeVisualsDatabase athleteTypeVisualsDatabase)
        {
            ItemPrefab = itemPrefab;
            AthleteTypes = athleteTypes;
            AthleteTypeVisualsDatabase = athleteTypeVisualsDatabase;
        }
    }
}