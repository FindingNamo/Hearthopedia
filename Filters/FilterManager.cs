﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class FilterManager
    {
        private List<ICardFilter> _ActiveFilters;
        
        private static FilterManager _instance;
        public static FilterManager Instance
        {
            get
            {
                return _instance ?? (_instance = new FilterManager());
            }
        }

        private FilterManager()
        {
            _ActiveFilters = new List<ICardFilter>()
            {
                CardClassFilter.Instance,
                CardQualityFilter.Instance, 
                CardRaceFilter.Instance,
                CardSetFilter.Instance,
                CardTypeFilter.Instance,
            };
        }
        
        /// <summary>
        /// Filters describe values which are acceptable to show.
        /// </summary>
        public bool Check(Card card)
        {
            foreach (ICardFilter filter in _ActiveFilters)
            {
                if (filter.Check(card))
                    return true;
            }

            // None of the filters succeeded, don't show this card.
            return false;
        }

    }
}
