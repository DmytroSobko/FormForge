using System.Threading.Tasks;
using FormForge.AssetManagement;
using FormForge.AssetManagement.AssetPolicy;
using FormForge.Core.Services;
using FormForge.Infrastructure.UI.Pagination;
using UnityEngine;

namespace FormForge.UI.Screens.Views.AthleteScreen
{
    public class AthletesPagination : PaginatedModule<AthleteItemPresenter, AthleteItemViewModel>
    {
        public override async void Initialize(IDataProvider<AthleteItemViewModel> provider, 
            string noContentMessage = "")
        {
            m_ItemPrefab = await LoadItemPrefab();
            base.Initialize(provider, noContentMessage);
        }
        
        private async Task<AthleteItemPresenter> LoadItemPrefab()
        {
            BasicAssetPolicy policy = new BasicAssetPolicy(AddressKeys.UI.AthletesScreen.AthleteItemView);
            GameObject itemView = await ServiceLocator.GetService<IAssetManagementService>().
                LoadAsync<GameObject, UIContext>(policy);
            
            return itemView.GetComponent<AthleteItemPresenter>();
        }
    }
}