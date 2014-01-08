using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia
{
    class Card
    {
        public int id { get; set; }
        public string image { get; set; }
        public int set { get; set; }
        public string icon { get; set; }
        public int type { get; set; }
        public int faction { get; set; }
        public int quality { get; set; }
        public int cost { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public int collectible { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<int> mechanics { get; set; }
    }
}
