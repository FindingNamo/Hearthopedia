using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Hearthopedia
{
    class DataManager
    {
        #region Public Properties

        public DateTime LastSearchTime { get; set; }

        public List<Mechanic> Mechanics { get; private set; }

        public ObservableCollection<Card> Cards { get; private set; }

        public ObservableCollection<Card> SearchedCards { get; set; }

        #endregion

        #region Constructor

        private DataManager()
        {
            this.SearchedCards = new ObservableCollection<Card>();
            this.Cards = new ObservableCollection<Card>();
            this.Mechanics = new List<Mechanic>();
        }

        #endregion

        #region Singleton Accessor

        private static DataManager dataManager;

        public static DataManager Instance
        {
            get
            {
                if (dataManager == null)
                    dataManager = new DataManager();

                return dataManager;
            }
        }

        #endregion

        #region Helpers

        public void SortCards()
        {
            this.Cards = new ObservableCollection<Card>(from i in this.Cards orderby i.name select i);
        }

        #endregion
    }
}
