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
            Card card = JsonConvert.DeserializeObject<Card>(json);
            return card;
        }

        
    }
}
