using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;

namespace Hearthopedia
{
    class TierListManager
    {
        #region Public Properties

        public CardTier.TierSource ActiveTierSource { get; set; }

        public CardTier.TierClass ActiveTierClass { get; set; }

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

        public int GetTierFromCard(Card card)
        {
           
            List<CardTier> tierList;

            // load from local
            using (StreamReader reader = new StreamReader(GetLocalTierListPath(this.ActiveTierClass)))
            {
                string tierJson = reader.ReadToEnd();

                tierList = GetTiersFromJson(tierJson);
            }

            foreach (CardTier cardTier in tierList)
            {
                if (cardTier.name.ToLower() == card.name.ToLower())
                    return cardTier.tier;
            }

            // if we get -1 it's all bad
            return -1;
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