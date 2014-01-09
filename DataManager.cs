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
        private ObservableCollection<Card> cards = new ObservableCollection<Card>();
        private ObservableCollection<Card> displayedCards = new ObservableCollection<Card>();
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

        public DateTime LastSearchTime { get; set; }

        public ObservableCollection<Card> Cards
        {
            get
            {
                return cards;
            }
        }

        public ObservableCollection<Card> DisplayedCards
        {
            get
            {
                return displayedCards;
            }
        }
    }
}
