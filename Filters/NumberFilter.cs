using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hearthopedia.Filters
{
    public class NumberFilter : ICardFilter, INotifyPropertyChanged
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

        private int? _minHealth;
        public int? MinHealth
        {
            get
            {
                return _minHealth;
            }
            set
            {
                if (_minHealth != value)
                {
                    _minHealth = value;
                    OnPropertyChanged("MinHealth");
                }
            }
        }

        private int? _maxHealth;
        public int? MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                if (_maxHealth != value)
                {
                    _maxHealth = value;
                    OnPropertyChanged("MaxHealth");
                }
            }
        }

        private int? _minCost;
        public int? MinCost
        {
            get
            {
                return _minCost;
            }
            set
            {
                if (_minCost != value)
                {
                    _minCost = value;
                    OnPropertyChanged("MinCost");
                }
            }
        }

        private int? _maxCost;
        public int? MaxCost
        {
            get
            {
                return _maxCost;
            }
            set
            {
                if (_maxCost != value)
                {
                    _maxCost = value;
                    OnPropertyChanged("MaxCost");
                }
            }
        }

        private int? _minAttack;
        public int? MinAttack
        {
            get
            {
                return _minAttack;
            }
            set
            {
                if (_minAttack != value)
                {
                    _minAttack = value;
                    OnPropertyChanged("MinAttack");
                }
            }
        }

        private int? _maxAttack;
        public int? MaxAttack
        {
            get
            {
                return _maxAttack;
            }
            set
            {
                if (_maxAttack != value)
                {
                    _maxAttack = value;
                    OnPropertyChanged("MaxAttack");
                }
            }
        }

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

        private void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));

            FilterManager.Instance.Dirty = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
