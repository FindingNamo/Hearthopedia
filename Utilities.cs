using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Hearthopedia
{
    class Utilities
    {
        public static Card GetCardFromJson(string json)
        {
            json =  "{\"id\":12,\"image\":\"EX1_055\",\"set\":3,\"icon\":\"inv_misc_ticket_tarot_beasts_01\",\"type\":4,\"faction\":2,\"quality\":3,\"cost\":2,\"attack\":1,\"health\":3,\"collectible\":1,\"name\":\"Mana Addict\",\"description\":\"Whenever you cast a spell, gain +2 Attack this turn.\",mechanics:[52]}";
            Card card = JsonConvert.DeserializeObject<Card>(json);

            return card;
        }

        
    }
}
