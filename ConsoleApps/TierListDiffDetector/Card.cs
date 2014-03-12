using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Hearthscraper
{
    public class Card
    {
        public int id { get; set; }
        public string image { get; set; }

        public CardTier.TierRank tier { get; set; }


        // net url
        private string _imageUrl;
        public string imageURL
        {
            get
            {
                return _imageUrl ?? (_imageUrl = "http://wow.zamimg.com/images/hearthstone/cards/enus/medium/" + image + ".png");
            }
        }

        // backup uri of the file that we saved last time local uri
        private string _localFilename;
        public string localFilename
        {
            get
            {
                return _localFilename ?? (_localFilename = image + ".old.png");
            }
        }

        // last resort local uri
        private string _backupURI;
        public string backupURI
        {
            get
            {
                return _backupURI ?? (_backupURI = "ms-appx:///Assets/CardImages/" + image + ".png");
            }
        }


        public int durability { get; set; }
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
        private string _flavourTextURL;
        public string flavourTextURL
        {
            get
            {
                return _flavourTextURL ?? (_flavourTextURL = "http://www.hearthhead.com/card=" + id + "&power");
            }
        }
        
        public string CostIconPath
        {
#if NETFX_CORE
            get { return "Assets/Sprites/blue32.png"; }
#else
            get { return "Assets\\Sprites\\blue32.png"; }
#endif
        }

        public string AttackIconPath
        {
#if NETFX_CORE
            get { return type == 7 ? "Assets/Sprites/weapon32.png" : "Assets/Sprites/yellow32.png"; }
#else
            get { return type == 7 ? "Assets\\Sprites\\weapon32.png" : "Assets\\Sprites\\yellow32.png"; }
#endif
        }

        public string HealthIconPath
        {
#if NETFX_CORE
            get { return type == 7 ? "Assets/Sprites/durability32.png" : "Assets/Sprites/red32.png"; }
#else
            get { return type == 7 ? "Assets\\Sprites\\durability32.png" : "Assets\\Sprites\\red32.png"; }
#endif
        }

        public string AttackLabel
        {
            get { return type == 7 ? "Weapon Damage" : "Attack"; }
        }

        public string HealthLabel
        {
            get { return type == 7 ? "Durability" : "Health"; }
        }

               

        /// <summary>
        /// Get a hex colour string for the quality of this card.
        /// </summary>
        public string QualityColourHexString
        {
            get
            {
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

    }
}
