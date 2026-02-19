using System.Collections.Generic;

namespace FormForge.Infrastructure.UI.Pagination
{
    public interface IPaginatedDataProvider<TItemViewModel> where TItemViewModel : IPaginatedItemViewModel
    {
        public IReadOnlyList<TItemViewModel> Items { get; }
        string NoContentMessage { get; }
        PageResult<TItemViewModel> GetPage(int pageIndex, int pageSize);
    }

    public struct PageResult<T>
    {
        public List<T> Items;
        public int TotalItemCount;

        public PageResult(List<T> items, int totalItemCount)
        {
            Items = items;
            TotalItemCount = totalItemCount;
        }
    }
}