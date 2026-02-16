using FormForge.Infrastructure.UI.Screens.Views;
using UnityEngine;

namespace FormForge.UI.Screens.Views.AthleteScreen
{
    public class AthletesScreenView : BaseScreenView
    {
        [SerializeField] private AthletesPagination m_Pagination;

        public void InitView(AthletesDataProvider dataProvider, string noContentMessage = "")
        {
            m_Pagination.Initialize(dataProvider, noContentMessage);
        }
    }
}