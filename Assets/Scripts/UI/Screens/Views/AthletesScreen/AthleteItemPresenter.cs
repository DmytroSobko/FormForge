using System.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Core.Services;
using FormForge.Domain;
using FormForge.Infrastructure.UI.Pagination;
using FormForge.ScriptableObjects.Athletes;
using UnityEngine;

namespace FormForge.UI.Screens.Views.AthleteScreen
{
    public class AthleteItemPresenter : MonoBehaviour, IPaginatedItemPresenter<AthleteItemViewModel>
    {
        [SerializeField] private AthleteItemView m_AthleteItemView;
        
        public async void Bind(AthleteItemViewModel viewModel)
        {
            Sprite athleteIcon = await LoadAthleteIcon(viewModel.AthleteType);
            
            m_AthleteItemView.InitView(viewModel, athleteIcon, OnItemClicked);
        }

        private async Task<Sprite> LoadAthleteIcon(EAthleteType athleteType)
        {
            BasicAssetPolicy policy = new BasicAssetPolicy(GetAthleteConfigAddress(athleteType));
            AthleteDefinitionSO athleteDef = await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<AthleteDefinitionSO, UIContext>(policy);
            
            return athleteDef.Icon;
        }

        private string GetAthleteConfigAddress(EAthleteType athleteType)
        {
            string address = athleteType switch
            {
                EAthleteType.Balanced => AddressKeys.Configs.BalancedAthlete,
                EAthleteType.EnduranceFocused => AddressKeys.Configs.EnduranceFocusedAthlete,
                EAthleteType.StrengthFocused => AddressKeys.Configs.StrengthFocusedAthlete,
                _ => string.Empty
            };

            return address;
        }

        private void OnItemClicked()
        {
            // TODO show tooltip with profile details
        }
    }
}