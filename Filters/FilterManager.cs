using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class FilterManager
    {
        private List<ICardFilter> _activeFilters;
        
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
            _activeFilters = new List<ICardFilter>();
        }
        
        /// <summary>
        /// Tests the passed in card against all active filters.
        /// </summary>
        public bool Check(Card card)
        {
            foreach (ICardFilter filter in _activeFilters)
            {
                if (!filter.Check(card))
                    return false;
            }

            // None of the filters failed, so it passes.
            return true;
        }

        public void AddFilter(ICardFilter filter)
        {
            _activeFilters.Add(filter);
        }

        public void ClearFilters()
        {
            // Do i have to call dispose or does that happen?
            foreach (ICardFilter filter in _activeFilters)
                filter.Dispose();

            _activeFilters.Clear();
        }

        /// <summary>
        /// Gets the active filters
        /// Is there a keyword for you can't edit this list, this should have that.
        /// </summary>
        public List<ICardFilter> GetActiveFilters()
        {
            return _activeFilters;
        }

    }
}
