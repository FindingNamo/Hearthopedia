using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardTypeFilter : AEnumNumberFilter<CardTypes>
    {
        private CardTypeFilter()
        {
            _FilterOptions = EnumUtilities.AssembleEnumerableOptionList<CardTypes>();
        }

        private static CardTypeFilter _instance;
        public static CardTypeFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new CardTypeFilter());
            }
        }

        public override bool Check(Card card)
        {
            return CheckEnum(card.type);
        }
    }
}
