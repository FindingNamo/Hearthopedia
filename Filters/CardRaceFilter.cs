using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardRaceFilter : AEnumNumberFilter<CardRace>
    {
        private CardRaceFilter()
        {
            _FilterOptions = EnumUtilities.AssembleEnumerableOptionList<CardRace>();
        }

        private static CardRaceFilter _instance;
        public static CardRaceFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new CardRaceFilter());
            }
        }
        public override bool Check(Card card)
        {
            return CheckEnum(card.classs);
        }
    }
}
