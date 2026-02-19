using System;
using FormForge.Infrastructure.UI.Screens.ViewModels;

namespace FormForge.Infrastructure.UI.Selection
{
    public interface ISelectableItem<out T> where T : IItemViewModel
    {
        T ViewModel { get; }
        
        event Action<ISelectableItem<T>> ItemSelected;

        void SetSelected(bool selected);
    }
}