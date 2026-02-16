using System;
using System.Collections.Generic;
using System.Linq;
using FormForge.Infrastructure.UI.Pagination;
using FormForge.Runtime.Models.Athletes;

namespace FormForge.UI.Screens.Views.AthleteScreen
{
    public class AthletesDataProvider : IDataProvider<AthleteItemViewModel>
    {
        public IReadOnlyList<AthleteItemViewModel> Items { get; }

        public AthletesDataProvider(IReadOnlyList<Athlete> athletes)
        {
            Items = athletes.Select(a => new AthleteItemViewModel(a.AthleteType, a.DisplayName)).ToList();
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