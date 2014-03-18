using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

#if NETFX_CORE
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using System.IO;
#else
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace Hearthopedia
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
#if NETFX_CORE
                return _backupURI ?? (_backupURI = "ms-appx:///Assets/CardImages/" + image + ".png");
#else
                return _backupURI ?? (_backupURI = "Assets/CardImages/" + image + ".png");
#endif
            }
        }

        public int HealthOrDurability
        {
            get
            {
                if (type == (int)CardTypes.Weapon)
                    return durability;

                return health;
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
        private List<Mechanic> _mechanicData;
        public List<Mechanic> MechanicData
        {
            get
            {
                return _mechanicData ?? (_mechanicData = ParseMechanics());
            }
        }

        private List<Mechanic> ParseMechanics()
        {
            List<Mechanic> newMechanics = new List<Mechanic>();

            if (mechanics != null)
            {
                foreach (int mechanicId in mechanics)
                {
                    newMechanics.Add(DataManager.Instance.Mechanics.Find( m => m.Id == mechanicId));
                }
            }
            else
            {
                newMechanics.Add(Mechanic.None);
            }

            return newMechanics;
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

        public string Dump
        {
            get
            {
                return string.Format("Set: {0}\nType: {2}\nFaction: {3}\nClass: {4}\nQuality: {5}\nCost: {6}\nAttack: {7}\nHealth: {8}\n, Collectible: {9}\n, Description: {10}\n, Mechanics: {11}\nRace: {12}",
                    CardSetString, icon, CardTypeString, faction, ClassNameString, CardQualityString, cost, attack, health, collectible, description, mechanics, CardRaceString);
            }
        }

        /// <summary>
        /// Gets a string representation of the race of this card.
        /// </summary>
        public string CardRaceString
        {
            get
            {
                return EnumUtilities.GetName<CardRace>((CardRace)race);
            }
        }

        /// <summary>
        /// Get a string representative of the Card's Set.
        /// </summary>
        public string CardSetString
        {
            get
            {
                return EnumUtilities.GetName<CardSet>((CardSet)set);
            }
        }

        /// <summary>
        /// Get a string representative of the Card Type.
        /// </summary>
        public string CardTypeString
        {
            get
            {
                return EnumUtilities.GetName<CardTypes>((CardTypes)type);
            }
        }

        /// <summary>
        /// Get a relative path to the portrait for the class of this card.
        /// </summary>
        public string ClassPortraitImagePath
        {
            get
            {
#if NETFX_CORE
                return "Assets/ClassPortraits/" + ClassNameString + ".png";
#else
               return "Assets\\ClassPortraits\\" + ClassNameString + ".png";
#endif
            }
        }

        /// <summary>
        /// Get a relative path to the portrait for the class of this card.
        /// </summary>
        public string ClassBannerImagePath
        {
            get
            {
#if NETFX_CORE
                return "Assets/ClassBanners/" + ClassNameString + ".png";
#else
                return "Assets\\ClassBanners\\" + ClassNameString + ".jpg";
#endif
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
                return EnumUtilities.GetName<CardQuality>((CardQuality)quality);
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
                return EnumUtilities.GetName<CardClass>((CardClass)classs);
            }
        }

        public string Tier
        {
            get
            {
                return TierListManager.Instance.GetTierFromCard(this).ToString();
            }
        }
    }
}
