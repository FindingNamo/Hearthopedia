using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hearthopedia;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Hearthopedia.Arena
{

    public class Arena : INotifyPropertyChanged
    {
        private int[] _manaCurve = new int[8];

        public int ManaCost0 { get { return _manaCurve[0]; } }
        public int ManaCost1 { get { return _manaCurve[1]; } }
        public int ManaCost2 { get { return _manaCurve[2]; } }
        public int ManaCost3 { get { return _manaCurve[3]; } }
        public int ManaCost4 { get { return _manaCurve[4]; } }
        public int ManaCost5 { get { return _manaCurve[5]; } }
        public int ManaCost6 { get { return _manaCurve[6]; } }
        public int ManaCost7 { get { return _manaCurve[7]; } }

        /// <summary>
        /// The mapping of quality to odds for normal rounds.
        /// </summary>
        public Dictionary<CardQuality, int> CommonRoundOdds;

        /// <summary>
        /// The mapping of quality to odds for uncommon rounds.
        /// </summary>
        public Dictionary<CardQuality, int> UncommonRoundOdds;

        /// <summary>
        /// A listing of which rounds of cards are uncommon.
        /// </summary>
        public List<int> UncommonRoundNumbers;

        /// <summary>
        /// Probability of picking a class specific card.
        /// </summary>
        public float ClassOdds = 0.3f;

        /// <summary>
        /// Number of cards per round
        /// </summary>
        public int CardsPerPick = 3;

        /// <summary>
        /// Total number of cards
        /// </summary>
        public int CardsPerDeck = 30;

        /// <summary>
        /// Valid types of cards to pick in arena mode.
        /// </summary>
        public List<CardTypes> ValidTypes;

        /// <summary>
        /// Valid card sets to choose from in arena mode.
        /// </summary>
        public List<CardSet> ValidCardSets;

        /// <summary>
        /// The bucket of cards specific to this class.
        /// </summary>
        public Dictionary<CardQuality, List<Card>> ClassSpecificCardBucket;

        /// <summary>
        /// The bucket of cards anyone could have.
        /// </summary>
        public Dictionary<CardQuality, List<Card>> NeutralCardBucket;

        /// <summary>
        /// Random number generator.
        /// </summary>
        private Random _random;

        /// <summary>
        /// The list of current cards.
        /// </summary>
        public ObservableCollection<Card> CurrentRoundCards 
        { 
            get; 
            set; 
        }
        
        /// <summary>
        /// The list of cards chosen.
        /// </summary>
        public ObservableCollection<Card> ChosenCards 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The Current Round number
        /// </summary>
        public int RoundNumber { get; set; }

        /// <summary>
        /// Creates an arena instance.
        /// </summary>
        /// <param name="classId"></param>
        public Arena(int classId)
        {
            _random = new Random();
            ChosenCards = new ObservableCollection<Card>();
            CurrentRoundCards = new ObservableCollection<Card>();

            SetupCommonRoundOdds();
            SetupUncommonRoundOdds();
            SetupUncommonRoundNumbers();
            SetupValidTypes();
            SetupValidCardSets();

            SetupCardBuckets(classId);

            RoundNumber = 0;
            AdvanceRound();
        }

        /// <summary>
        /// Advances to the next round and updates the observable lists.
        /// </summary>
        public void AdvanceRound()
        {
            CurrentRoundCards.Clear();
            List<Card> nextRoundCards = GetCardsForRound(++RoundNumber);
            foreach (Card c in nextRoundCards)
                CurrentRoundCards.Add(c);
        }

        /// <summary>
        /// Choose a card and advance to the next round.
        /// </summary>
        /// <param name="c"></param>
        public void ChooseCard(Card chosenCard)
        {
            AdvanceRound();

            int i = 0;
            for (i = 0; i < ChosenCards.Count; i++)
            {
                if (chosenCard.cost <= ChosenCards[i].cost)
                    break;
            }
            ChosenCards.Insert(i, chosenCard);


            int clampedMana = ClampInt(chosenCard.cost, 0, 7);
            _manaCurve[clampedMana] += 10;
            OnPropertyChanged(string.Format("ManaCost{0}", clampedMana));
        }


        private int ClampInt(int num, int min, int max)
        {
            if (num < min)
                return min;

            if (num > max)
                return max;

            return num;
        }

        /// <summary>
        /// Sets up the common round odds.
        /// </summary>
        public void SetupCommonRoundOdds()
        {
            CommonRoundOdds = new Dictionary<CardQuality, int>();
            CommonRoundOdds[CardQuality.Common] = 241 + 186;
            CommonRoundOdds[CardQuality.Rare] = 17 + 17;
            CommonRoundOdds[CardQuality.Epic] = 1 + 2;
            CommonRoundOdds[CardQuality.Legendary] = 1 + 3;
        }

        /// <summary>
        /// Sets up the uncommon round odds.
        /// </summary>
        public void SetupUncommonRoundOdds()
        {
            UncommonRoundOdds = new Dictionary<CardQuality,int>();
            UncommonRoundOdds[CardQuality.Rare] = 33 + 24;
            UncommonRoundOdds[CardQuality.Epic] = 5 + 4;
            UncommonRoundOdds[CardQuality.Legendary] = 2 + 4;
        }

        /// <summary>
        /// Sets up the Rounds which are uncommon.
        /// </summary>
        public void SetupUncommonRoundNumbers()
        {
            UncommonRoundNumbers = new List<int>()
            {
                1,
                10,
                20,
                30,
            };
        }

        /// <summary>
        /// Sets up which card types are valid for arena.
        /// </summary>
        public void SetupValidTypes()
        {
            ValidTypes = new List<CardTypes>()
            {
                CardTypes.Spell,
                CardTypes.Minion,
                CardTypes.Weapon,
            };
        }


        /// <summary>
        /// Sets up the list of valid card sets for arena.
        /// </summary>
        public void SetupValidCardSets()
        {
            ValidCardSets = new List<CardSet>()
            {
                CardSet.Basic,
                CardSet.Expert,
            };
        }

        /// <summary>
        /// Sets up the card buckets for this arena instance
        /// </summary>
        public void SetupCardBuckets(int classId)
        {
            ClassSpecificCardBucket = new Dictionary<CardQuality,List<Card>>();
            NeutralCardBucket = new Dictionary<CardQuality,List<Card>>();

            foreach(CardQuality q in Enum.GetValues(typeof(CardQuality)))
            {
                ClassSpecificCardBucket[q] = new List<Card>();
                NeutralCardBucket[q] = new List<Card>();
            }

            foreach (Card c in DataManager.Instance.Cards)
            {
                // Malformed cards, or uncollectable cards don't belong here.
                if (c.collectible == null || c.collectible == 0)
                    continue;

                // Unsupported card types don't belong here.
                if (!ValidTypes.Contains((CardTypes)c.type))
                    continue;

                // Unsupported card sets don't belong here.
                if (!ValidCardSets.Contains((CardSet)c.set))
                    continue;

                Dictionary<CardQuality,List<Card>> bucket;

                if (c.classs != null && c.classs == classId)
                    bucket = ClassSpecificCardBucket;
                else if (c.classs != null && c.classs == (int)CardClass.Everyone)
                    bucket = NeutralCardBucket;
                else
                    continue;

                // Bucket free cards in with Common cards.
                if ((CardQuality) c.quality == CardQuality.Free)
                    bucket[CardQuality.Common].Add(c);
                else
                    bucket[(CardQuality)c.quality].Add(c);
            }
        }

        /// <summary>
        /// Randomly decides on a CardQuality for a round.
        /// </summary>
        public CardQuality GetCardQualityForRound(Dictionary<CardQuality, int> qualityOdds)
        {
            int totalOdds = 0;
            foreach (int odds in qualityOdds.Values)
                totalOdds += odds;

            int chosenVal = (int)(_random.NextDouble() * totalOdds);

            CardQuality chosenQuality = CardQuality.Common;
            foreach (CardQuality currentQuality in qualityOdds.Keys)
            {
                chosenQuality = currentQuality;
                if (chosenVal < qualityOdds[currentQuality])
                    break;

                chosenVal -= qualityOdds[currentQuality];
            }

            return chosenQuality;
        }

        /// <summary>
        /// Gets a set of cards for a given round.
        /// </summary>
        public List<Card> GetCardsForRound(int roundNumber)
        {
            List<Card> chosenCardsForRound = new List<Card>();
            
            Dictionary<CardQuality, int> roundOdds;
            if (UncommonRoundNumbers.Contains(roundNumber))
                roundOdds = UncommonRoundOdds;
            else
                roundOdds = CommonRoundOdds;

            // Keep choosing a round quality until we have enough cards.
            CardQuality roundQuality;
            do
            {
                roundQuality = GetCardQualityForRound(roundOdds);
            } while (ClassSpecificCardBucket[roundQuality].Count + NeutralCardBucket[roundQuality].Count <= CardsPerPick);

            // Pick the cards
            Card chosenCard;
            for (int i = 0; i < CardsPerPick; i++)
            {
                Dictionary<CardQuality, List<Card>> bucket;

                // Make sure we don't draw the same card multiple times in the same round
                do
                {
                    if ((ClassSpecificCardBucket[roundQuality].Count > 0) && (_random.NextDouble() < ClassOdds))
                        bucket = ClassSpecificCardBucket;
                    else
                        bucket = NeutralCardBucket;

                    chosenCard = bucket[roundQuality][_random.Next(bucket[roundQuality].Count)];
                } while (chosenCardsForRound.Contains(chosenCard));
                
                chosenCardsForRound.Add(chosenCard);
            }

            return chosenCardsForRound;
        }


        private void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
