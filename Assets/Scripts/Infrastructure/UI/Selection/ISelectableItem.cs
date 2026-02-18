using System;

namespace FormForge.Infrastructure.UI.Selection
{
    public interface ISelectableItem<T>
    {
        T Value { get; }
        bool IsSelected { get; }

        event Action<ISelectableItem<T>> OnSelected;

        void SetSelected(bool selected);
    }
}