using System;
using System.Collections.Generic;

namespace FormForge.Infrastructure.UI.Selection
{
    public class SingleSelectionController<T>
    {
        private readonly List<ISelectableItem<T>> m_Items = new List<ISelectableItem<T>>();
        private ISelectableItem<T> m_Current;

        public T SelectedValue => m_Current != null ? m_Current.Value : default;
        public bool HasSelection => m_Current != null;

        public event Action<T> OnSelectionChanged;

        public void Register(ISelectableItem<T> item)
        {
            m_Items.Add(item);
            item.OnSelected += HandleSelection;
        }

        public void Unregister(ISelectableItem<T> item)
        {
            item.OnSelected -= HandleSelection;
            m_Items.Remove(item);

            if (m_Current == item)
            {
                m_Current = null;
            }
        }

        private void HandleSelection(ISelectableItem<T> selected)
        {
            if (m_Current == selected)
            {
                return;
            }

            m_Current?.SetSelected(false);

            m_Current = selected;
            m_Current.SetSelected(true);

            OnSelectionChanged?.Invoke(m_Current.Value);
        }

        public void ClearSelection()
        {
            if (m_Current == null)
            {
                return;
            }
            m_Current.SetSelected(false);
            m_Current = null;
        }
    }
}