using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class FilterManager
    {
        // Is there a way I can stop people from calling methods on this directly?
        // Not important for us, but I'd like to know for my own business with C#.
        public readonly ObservableCollection<ICardFilter> ActiveFilters;
        
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
            ActiveFilters = new ObservableCollection<ICardFilter>();
        }
        
        /// <summary>
        /// Tests the passed in card against all active filters.
        /// </summary>
        public bool Check(Card card)
        {
            foreach (ICardFilter filter in ActiveFilters)
            {
                if (!filter.Check(card))
                    return false;
            }

            // None of the filters failed, so it passes.
            return true;
        }

        public void AddFilter(ICardFilter filter)
        {
            ActiveFilters.Add(filter);
        }

        public void ClearFilters()
        {
            // Do i have to call dispose or does that happen?
            foreach (ICardFilter filter in ActiveFilters)
                filter.Dispose();

            ActiveFilters.Clear();
        }
    }
}
