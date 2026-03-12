using System.Collections.Generic;
using FormForge.Infrastructure.Collections;
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
        private Pool<PoolableObject> m_Pool;
        private List<PoolableObject> m_AcquiredPoolItems = new List<PoolableObject>();

        private int m_CurrentPage;
        private int m_TotalPages;
        private int m_TotalItemCount;
        
        public void Initialize(IPaginatedDataProvider<TItemViewModel> provider, GameObject itemPrefab)
        {
            m_PaginatedDataProvider = provider;
            
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
                m_Pool = new Pool<PoolableObject>(m_PageSize, itemPrefab);

                m_NextButton.onClick.AddListener(OnNextClicked);
                m_PrevButton.onClick.AddListener(OnPrevClicked);
                
                GoToPage(0);
            }
        }

        private void GoToPage(int pageIndex)
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
                RecyclePoolObjects();

                foreach (var itemData in result.Items)
                {
                    PoolableObject item = m_Pool.Acquire();
                    item.transform.SetParent(m_ContentRoot, false);
                    item.GetComponent<TItemPresenter>().Initialize(itemData);
                    m_AcquiredPoolItems.Add(item);
                }

                m_CurrentPage = pageIndex;
                UpdateUI();
            });
        }

        private void RecyclePoolObjects()
        {
            foreach (PoolableObject item in m_AcquiredPoolItems)
            {
                item.Recycle();
            }
            
            m_AcquiredPoolItems.Clear();
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