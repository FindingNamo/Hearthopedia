using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia
{
    class DataManager
    {
        private List<Card> cards = new List<Card>();
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

        public List<Card> Cards
        {
            get
            {
                return cards;
            }
        }
    }
}
