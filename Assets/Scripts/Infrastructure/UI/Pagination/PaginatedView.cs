using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FormForge.Infrastructure.UI.Pagination
{
    public abstract class PaginatedView<TItemPresenter, TItemViewModel> : MonoBehaviour
        where TItemPresenter : MonoBehaviour, IPaginatedItemPresenter<TItemViewModel>
        where TItemViewModel : IPaginatedItemViewModel
    {
        [SerializeField] private Transform m_ContentRoot;
        [SerializeField] private Button m_NextButton;
        [SerializeField] private Button m_PrevButton;
        [SerializeField] private TextMeshProUGUI m_NoContentText;
        [SerializeField] private TextMeshProUGUI m_PageIndicatorText;
        [SerializeField] private PageAnimator m_Animator;
        [SerializeField] private int m_PageSize;

        private IPaginatedDataProvider<TItemViewModel> m_PaginatedDataProvider;
        private ItemPool<TItemPresenter> m_Pool;

        protected TItemPresenter m_ItemPrefab;

        private int m_CurrentPage;
        private int m_TotalPages;
        private int m_TotalItemCount;
        
        public void Initialize(IPaginatedDataProvider<TItemViewModel> provider, GameObject itemPrefab)
        {
            m_PaginatedDataProvider = provider;
            m_ItemPrefab = itemPrefab.GetComponent<TItemPresenter>();
            
            if (m_PaginatedDataProvider.Items.Count == 0)
            {
                m_NextButton.gameObject.SetActive(false);
                m_PrevButton.gameObject.SetActive(false);
                m_PageIndicatorText.gameObject.SetActive(false);
                m_NoContentText.gameObject.SetActive(true);
                m_NoContentText.text = m_PaginatedDataProvider.NoContentMessage;
            }
            else
            {
                m_Pool = new ItemPool<TItemPresenter>(m_ItemPrefab, m_ContentRoot);

                m_NextButton.onClick.AddListener(OnNextClicked);
                m_PrevButton.onClick.AddListener(OnPrevClicked);
                
                GoToPage(0);
            }
        }

        public void GoToPage(int pageIndex)
        {
            if (pageIndex < 0)
            {
                return;
            }

            if (m_TotalPages > 0 && pageIndex >= m_TotalPages)
            {
                return;
            }

            LoadPage(pageIndex);
        }

        private void LoadPage(int pageIndex)
        {
            PageResult<TItemViewModel> result = m_PaginatedDataProvider.GetPage(pageIndex, m_PageSize);

            m_TotalItemCount = result.TotalItemCount;
            m_TotalPages = Mathf.CeilToInt((float) m_TotalItemCount / m_PageSize);

            int direction = pageIndex > m_CurrentPage ? 1 : -1;

            m_Animator.Animate(direction, () =>
            {
                m_Pool.ReleaseAll();

                foreach (var itemData in result.Items)
                {
                    var item = m_Pool.Get();
                    item.Initialize(itemData);
                }

                m_CurrentPage = pageIndex;
                UpdateUI();
            });
        }

        private void UpdateUI()
        {
            m_PageIndicatorText.text = $"{m_CurrentPage + 1} / {m_TotalPages}";

            m_PrevButton.interactable = m_CurrentPage > 0;
            m_NextButton.interactable = m_CurrentPage < m_TotalPages - 1;
        }

        private void OnNextClicked()
        {
            GoToPage(m_CurrentPage + 1);
        }

        private void OnPrevClicked()
        {
            GoToPage(m_CurrentPage - 1);
        }
    }
}