using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hearthopedia.Filters
{
    public class NumberFilter : ICardFilter
    {
        private NumberFilter()
        {
        }

        private static NumberFilter _instance;
        public static NumberFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new NumberFilter());
            }
        }

        public int? MinHealth { set; get; }
        public int? MaxHealth { set; get; }

        public int? MinCost { set; get; }
        public int? MaxCost { set; get; }

        public int? MinAttack { set; get; }
        public int? MaxAttack { set; get; }

        public bool Check(Card card)
        {
            bool passesFilter = true;

            // Health
            if (MinHealth.HasValue)
                passesFilter = passesFilter && card.health >= MinHealth;
            if (MaxHealth.HasValue)
                passesFilter = passesFilter && card.health <= MaxHealth;

            // Cost
            if (MinCost.HasValue)
                passesFilter = passesFilter && card.health >= MinCost;
            if (MaxCost.HasValue)
                passesFilter = passesFilter && card.health <= MaxCost;

            // Attack
            if (MinAttack.HasValue)
                passesFilter = passesFilter && card.health >= MinAttack;
            if (MaxAttack.HasValue)
                passesFilter = passesFilter && card.health <= MaxAttack;

            return passesFilter;
        }

        /// <summary>
        /// Clear the filter.
        /// </summary>
        public void SetUncheckedAll()
        {
            MinAttack = null;
            MinCost = null;
            MinHealth = null;

            MaxAttack = null;
            MaxCost = null;
            MaxHealth = null;
        }

        /// <summary>
        /// What would that even mean?
        /// </summary>
        public void SetCheckedAll()
        {
        }
    }
}
