using UnityEngine;

namespace FormForge.Infrastructure.UI.Screens.Views
{
    public abstract class BaseScreenView : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
    }
}