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

        private string _imageUrl;
        public string imageURL
        {
            get
            {
                return _imageUrl ?? (_imageUrl = "http://wow.zamimg.com/images/hearthstone/cards/enus/medium/" + image + ".png");
            }
        }

        public int set { get; set; }
        public string icon { get; set; }
        public int type { get; set; }
        public int faction { get; set; }
        public int classs { get; set; }
        public int quality { get; set; }
        public int cost { get; set; }
        public int attack { get; set; }
        public int health { get; set; }
        public int collectible { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<int> mechanics { get; set; }

        public string ClassPortraitImagePath
        {
            get
            {
               return "Assets\\ClassPortraits\\" + ClassNameString + ".png";
            }
        }

        public string QualityColourHexString
        {
            get{
                switch (quality)
                {
                case 0:
                    return "#ff9d9d9d";
                case 1:
                    return "#ffffffff";
                case 2:
                    return "#ff1eff00";
                case 3:
                   return "#ff0070dd";
                case 4:
                    return "#ffa335ee";
                case 5:
                    return "#ffff8000";
                case 6:
                    return "#ffe6cc80";
                case 7:
                     return "#ffe5cc80";
                case 8:
                    return "#ffffff98";
                case 9:
                    return "#ff71d5ff";
                case 10:
                    return "#ffff4040";
                default:
                    return "#FFFF00FF";
                }
            }
        }

        public string ClassNameString
        {
            get
            {
                switch (classs)
                {
                    case 1:
                        return "Warrior";
                    case 2:
                        return "Paladin";
                    case 3:
                        return "Hunter";
                    case 4:
                        return "Rogue";
                    case 5:
                        return "Priest";
                    // no 6
                    case 7:
                        return "Shaman";
                    case 8:
                        return "Mage";
                    case 9:
                        return "Warlock";
                    // no 10;
                    case 11:
                        return "Druid";
                    default:
                        return "Unknown";
                }   
            }
        }
    }
}
