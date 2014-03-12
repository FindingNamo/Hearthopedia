using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthscraper.Settings
{
    class Hearthhead
    {
        // define constant part of the URL: http://www.hearthhead.com/card=
        public readonly string baseURL = "http://www.hearthhead.com/card=";

        // define url params needed to return "full" info
        public readonly string powerURL = "&power";

        // define url param needed to return "basic" info
        public readonly string textURL = "&text";
    }
}
