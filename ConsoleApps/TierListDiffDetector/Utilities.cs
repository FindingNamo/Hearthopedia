using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hearthscraper
{
    class Utilities
    {
        public static Card GetCardFromJson(string json)
        {
            Card card = JsonConvert.DeserializeObject<Card>(json);
            return card;
        }

        public static List<CardTier> GetTiersFromJson(string json)
        {
            List<CardTier> cardsTiers = new List<CardTier>();
            cardsTiers = JsonConvert.DeserializeObject<List<CardTier>>(json);
            return cardsTiers;
        }
    }
}
