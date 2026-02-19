using System;
using System.Collections.Generic;
using System.Linq;
using FormForge.Infrastructure.UI.Pagination;
using FormForge.UI.Screens.ViewModels.AthletesScreen;

namespace FormForge.UI.Screens.Pagination.DataProviders
{
    public class AthletesPaginatedDataProvider : IPaginatedDataProvider<AthleteItemViewModel>
    {
        public IReadOnlyList<AthleteItemViewModel> Items { get; }
        public string NoContentMessage { get; }

        public AthletesPaginatedDataProvider(IReadOnlyList<AthleteItemViewModel> items, string noContentMessage = "")
        {
            Items = items;
            NoContentMessage = noContentMessage;
        }

        public PageResult<AthleteItemViewModel> GetPage(int pageIndex, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentException("pageSize must be greater than 0");
            }

            if (pageIndex < 0)
            {
                throw new ArgumentException("pageIndex cannot be negative");
            }

            int totalItemCount = Items.Count;
            int startIndex = pageIndex * pageSize;

            if (startIndex >= totalItemCount)
            {
                return new PageResult<AthleteItemViewModel>(new List<AthleteItemViewModel>(), totalItemCount);
            }

            int count = Math.Min(pageSize, totalItemCount - startIndex);

            var items = Items
                .Skip(startIndex)
                .Take(count)
                .ToList();

            return new PageResult<AthleteItemViewModel>(items, totalItemCount);
        }
    }
}