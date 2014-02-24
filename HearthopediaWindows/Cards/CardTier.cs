using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia
{
    public class CardTier
    {
        public enum TierRank
        {
            Top = 1,
            Great = 2,
            Good = 3,
            Average = 4,
            Bad = 5,
            Terrible = 6
        }

        public enum TierSource
        {
            Trump,
            Antigravity
        }

        public enum TierClass
        {
            Druid,
            Hunter,
            Mage,
            Paladin,
            Priest,
            Rogue,
            Shaman,
            Warlock,
            Warrior
        }

        public string name { get; set; }
        public int tier { get; set; }
    }

 
}
