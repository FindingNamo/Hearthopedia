using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    /// <summary>
    /// Creates the filters for us based off enum.
    /// </summary>
    public static class FilterFactory
    {
        public static ICardFilter GetFilter(FilterType type)
        {
            switch (type)
            {
                case FilterType.Cost:
                    return new CardCostFilter();
                case FilterType.Attack:
                    return new CardAttackFilter();
                case FilterType.Health:
                    return new CardHealthFilter();
                case FilterType.Type:
                    return new CardTypeFilter();

                case FilterType.Set:
                    return new CardSetFilter();

                case FilterType.Race:
                    return new CardRaceFilter();

                case FilterType.Quality:
                    return new CardQualityFilter();

                case FilterType.Class:
                    return new CardClassFilter();

                default:
                    throw new ArgumentException();
            }
        }
    }
}