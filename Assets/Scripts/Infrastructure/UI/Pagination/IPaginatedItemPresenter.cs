namespace FormForge.Infrastructure.UI.Pagination
{
    public interface IPaginatedItemPresenter<in TItemViewModel> : IPresenter
        where TItemViewModel : IPaginatedItemViewModel
    {
        void Initialize(TItemViewModel viewModel);
    }
}