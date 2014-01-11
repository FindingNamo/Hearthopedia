using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public int race { get; set; }
        public string description { get; set; }
        public List<int> mechanics { get; set; }

        public string Dump
        {
            get
            {
                return string.Format("Set: {0}\nType: {2}\nFaction: {3}\nClass: {4}\nQuality: {5}\nCost: {6}\nAttack: {7}\nHealth: {8}\n, Collectible: {9}\n, Description: {10}\n, Mechanics: {11}\nRace: {12}",
                    CardSetString, icon, CardTypeString, faction, classs, CardQualityString, cost, attack, health, collectible, description, mechanics, CardRaceString);
            }
        }

        /// <summary>
        /// Gets a string representation of the race of this card.
        /// </summary>
        public string CardRaceString
        {
            get
            {
                switch (race)
                {
                    case 0:
                        return "None";
                    case 20:
                        return "Beast";
                    case 15:
                        return "Demon";
                    case 24:
                        return "Dragon";
                    case 14:
                        return "Murloc";
                    case 23:
                        return "Pirate";
                    case 21:
                        return "Totem";
                    default:
                        return "Unknown";
                }
            }
        }

        /// <summary>
        /// Get a string representative of the Card's Set.
        /// </summary>
        public string CardSetString
        {
            get
            {
                switch (set)
                {
                    case 2:
                        return "Basic";
                    case 3:
                        return "Expert";
                    case 4:
                        return "Reward";
                    case 5:
                        return "Missions";
                    case 11:
                        return "Promotions";
                    default:
                        return "Unknown";
                }
            }
        }

        /// <summary>
        /// Get a string representative of the Card Type.
        /// </summary>
        public string CardTypeString
        {
            get
            {
                switch (type)
                {
                    case 3:
                        return "Hero";
                    case 4:
                        return "Minion";
                    case 5:
                        return "Spell";
                    case 7:
                        return "Weapon";
                    case 10:
                        return "Hero Power";
                    default:
                        return "Unknown";
                }
            }
        }

        /// <summary>
        /// Get a relative path to the portrait for the class of this card.
        /// </summary>
        public string ClassPortraitImagePath
        {
            get
            {
               return "Assets\\ClassPortraits\\" + ClassNameString + ".png";
            }
        }

        /// <summary>
        /// Get a relative path to the portrait for the class of this card.
        /// </summary>
        public string ClassBannerImagePath
        {
            get
            {
                return "Assets\\ClassBanners\\" + ClassNameString + ".jpg";
            }
        }

        public ImageBrush ClassBannerBrush
        {
            get
            {
                return new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(ClassBannerImagePath, UriKind.Relative)),
                    Stretch = Stretch.UniformToFill,
                    Opacity = 0.3,
                };
            }
        }

        /// <summary>
        /// Get a string name for the quality of this card.
        /// </summary>
        public string CardQualityString
        {
            get
            {
                switch (quality)
                {
                    case 0:
                        return "Free";
                    case 1:
                        return "Common";
                    case 2:
                        return "Magic";
                    case 3:
                        return "Rare";
                    case 4:
                        return "Epic";
                    case 5:
                        return "Legendary";
                    default:
                        return "Unknown";
                }
            }
        }

        /// <summary>
        /// Get a hex colour string for the quality of this card.
        /// </summary>
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

        /// <summary>
        /// Get the string name of the Class of this card.
        /// </summary>
        public string ClassNameString
        {
            get
            {
                switch (classs)
                {
                    case 0:
                        return "Everyone";
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
