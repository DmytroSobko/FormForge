using System;
using System.Collections.Generic;
using System.Linq;

namespace FormForge.Infrastructure.UI.Pagination
{
    public class PaginatedDataProvider<TItemViewModel> : IPaginatedDataProvider<TItemViewModel>
        where TItemViewModel : IPaginatedItemViewModel
    {
        public IReadOnlyList<TItemViewModel> Items { get; }
        public string NoContentMessage { get; }

        public PaginatedDataProvider(IReadOnlyList<TItemViewModel> items, string noContentMessage = "")
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            NoContentMessage = noContentMessage;
        }

        public PageResult<TItemViewModel> GetPage(int pageIndex, int pageSize)
        {
            if (pageSize <= 0)
                throw new ArgumentException("pageSize must be greater than 0", nameof(pageSize));

            if (pageIndex < 0)
                throw new ArgumentException("pageIndex cannot be negative", nameof(pageIndex));

            int totalItemCount = Items.Count;
            int startIndex = pageIndex * pageSize;

            if (startIndex >= totalItemCount)
                return new PageResult<TItemViewModel>(Array.Empty<TItemViewModel>().ToList(), totalItemCount);

            var items = Items
                .Skip(startIndex)
                .Take(pageSize)
                .ToList();

            return new PageResult<TItemViewModel>(items, totalItemCount);
        }
    }
}