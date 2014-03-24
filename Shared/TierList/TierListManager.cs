using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using Windows.Storage;

namespace Hearthopedia
{
    class TierListManager
    {
        #region Public Properties

        public CardTier.TierSource ActiveTierSource { get; set; }

        private CardTier.TierClass _activeTierClass;

        public CardTier.TierClass ActiveTierClass {
            get
            {
                return _activeTierClass;
            }
            set
            {
                _activeTierClass = value;
                this.UpdateCardTiers();
            }
        }

        public List<CardTier> CardTiers { get; private set; }

        #endregion

        #region Constructor

        private TierListManager()
        {
            this.ActiveTierSource = CardTier.TierSource.Antigravity;
            this.ActiveTierClass = CardTier.TierClass.Druid;
        }

        #endregion

        #region Singleton Accessor

        private static TierListManager tierListManager;

        public static TierListManager Instance
        {
            get
            {
                return tierListManager ?? (tierListManager = new TierListManager());
            }
        }

        #endregion

        public async Task UpdateCardTiers()
        {
            // Read from local
            using (StreamReader reader = new StreamReader(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(GetLocalTierListPath(this.ActiveTierClass))))
            {
                this.CardTiers = GetTiersFromJson(reader.ReadToEnd());
            }

            // Read from Web and update local file

            // Read from resources as a last resort
            Stream resourceStream = Application.GetResourceStream(GetTierListResourceUri(this.ActiveTierClass)).Stream;
            using (StreamReader reader = new StreamReader(resourceStream))
            {
                string tierJson = reader.ReadToEnd();

                this.CardTiers = GetTiersFromJson(tierJson);
            }
        }

        public int GetTierFromCard(Card card)
        {
            foreach (CardTier cardTier in this.CardTiers)
            {
                if (cardTier.name.ToLower() == card.name.ToLower())
                    return cardTier.tier;
            }

            // if we get -1 it's all bad
            return -1;
        }

        /// <summary>
        /// Given a set of cards, return the best tier of all of them.
        /// </summary>
        public int GetTopTierPick(IEnumerable<Card> cards)
        {
            int bestVal = (int) CardTier.TierRank.Terrible;
            foreach (Card c in cards)
            {
                int currentTier = GetTierFromCard(c);
                if (currentTier > 0)
                    bestVal = Math.Min(bestVal, currentTier);
            }

            return bestVal;
        }

        public static string GetLocalTierListPath(CardTier.TierClass tierClass)
        {
            switch (tierClass)
            {
                case CardTier.TierClass.Druid:
                    return "antigravity_druid.json";
                case CardTier.TierClass.Hunter:
                    return "antigravity_hunter.json";
                case CardTier.TierClass.Mage:
                    return "antigravity_mage.json";
                case CardTier.TierClass.Paladin:
                    return "antigravity_paladin.json";
                case CardTier.TierClass.Priest:
                    return "antigravity_priest.json";
                case CardTier.TierClass.Rogue:
                    return "antigravity_rogue.json";
                case CardTier.TierClass.Shaman:
                    return "antigravity_shaman.json";
                case CardTier.TierClass.Warlock:
                    return "antigravity_warlock.json";
                case CardTier.TierClass.Warrior:
                    return "antigravity_warrior.json";
                default:
                    return "bad times son...";
            }
        }

        public static Uri GetTierListResourceUri(CardTier.TierClass tierClass)
        {
            string baseResourceUri = @"Hearthopedia;component/Assets/TierList/";

            switch (tierClass)
            {
                case CardTier.TierClass.Druid:
                    return new Uri(baseResourceUri + "antigravity_druid.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Hunter:
                    return new Uri(baseResourceUri + "antigravity_hunter.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Mage:
                    return new Uri(baseResourceUri + "antigravity_mage.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Paladin:
                    return new Uri(baseResourceUri + "antigravity_paladin.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Priest:
                    return new Uri(baseResourceUri + "antigravity_priest.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Rogue:
                    return new Uri(baseResourceUri + "antigravity_rogue.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Shaman:
                    return new Uri(baseResourceUri + "antigravity_shaman.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Warlock:
                    return new Uri(baseResourceUri + "antigravity_warlock.json", UriKind.RelativeOrAbsolute);
                case CardTier.TierClass.Warrior:
                    return new Uri(baseResourceUri + "antigravity_warrior.json", UriKind.RelativeOrAbsolute);
                default:
                    return null;
            }
        }

        public static string GetTierListURL(CardTier.TierClass tierClass, CardTier.TierSource tierSource)
        {
            string baseURL = @"http://stillatthebottom.com/hearthopedia/";
            switch (tierSource)
            {
                case CardTier.TierSource.Antigravity:
                    {
                        switch (tierClass)
                        {
                            case CardTier.TierClass.Druid:
                                return baseURL + "antigravity_druid.json";
                            case CardTier.TierClass.Hunter:
                                return baseURL + "antigravity_hunter.json";
                            case CardTier.TierClass.Mage:
                                return baseURL + "antigravity_mage.json";
                            case CardTier.TierClass.Paladin:
                                return baseURL + "antigravity_paladin.json";
                            case CardTier.TierClass.Priest:
                                return baseURL + "antigravity_priest.json";
                            case CardTier.TierClass.Rogue:
                                return baseURL + "antigravity_rogue.json";
                            case CardTier.TierClass.Shaman:
                                return baseURL + "antigravity_shaman.json";
                            case CardTier.TierClass.Warlock:
                                return baseURL + "antigravity_warlock.json";
                            case CardTier.TierClass.Warrior:
                                return baseURL + "antigravity_warrior.json";
                            default:
                                return "antigravity_all.json";
                        }
                        break;
                    }
                case CardTier.TierSource.Trump:
                    {
                        switch (tierClass)
                        {
                            case CardTier.TierClass.Druid:
                                return baseURL + "trump_druid.json";
                            case CardTier.TierClass.Hunter:
                                return baseURL + "trump_druid.json";
                            case CardTier.TierClass.Mage:
                                return baseURL + "trump_mage.json";
                            case CardTier.TierClass.Paladin:
                                return baseURL + "trump_paladin.json";
                            case CardTier.TierClass.Priest:
                                return baseURL + "trump_priest.json";
                            case CardTier.TierClass.Rogue:
                                return baseURL + "trump_rogue.json";
                            case CardTier.TierClass.Shaman:
                                return baseURL + "trump_shaman.json";
                            case CardTier.TierClass.Warlock:
                                return baseURL + "trump_warlock.json";
                            case CardTier.TierClass.Warrior:
                                return baseURL + "trump_warrior.json";
                            default:
                                return "trump_all.json";
                        }
                        break;
                    }
            }

            // if we get here... it's bad times...
            return "error";
        }

        public static List<CardTier> GetTiersFromJson(string json)
        {
            List<CardTier> cardsTiers = new List<CardTier>();
            cardsTiers = JsonConvert.DeserializeObject<List<CardTier>>(json);
            return cardsTiers;
        }
    }
}