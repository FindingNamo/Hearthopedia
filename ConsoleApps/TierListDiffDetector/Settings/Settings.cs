using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthscraper.Settings
{
    class Settings
    {
        // private version
        Hearthhead hearthhead;

        // property to access hearthhead
        public Hearthhead Hearthhead
        {
            get
            {
                if (hearthhead == null)
                    hearthhead = new Hearthhead();
                return hearthhead;
            }
        }

        // Constructor
        public Settings()
        {
        }
    }
}
