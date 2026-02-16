namespace FormForge.Infrastructure.UI.Pagination
{
    public interface IPaginatedItemPresenter<in TItemViewModel> 
        where TItemViewModel : IPaginatedItemViewModel
    {
        void Bind(TItemViewModel viewModel);
    }
}