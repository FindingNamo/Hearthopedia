using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardSetFilter : AEnumNumberFilter<CardSet>
    {
        private string _name = "Set";

        public string Name
        {
            get
            {
                return _name;
            }
        }

        private CardSetFilter()
        {
            _FilterOptions = EnumUtilities.AssembleEnumerableOptionList<CardSet>();
        }

        private static CardSetFilter _instance;
        public static CardSetFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new CardSetFilter());
            }
        }
        public override bool Check(Card card)
        {
            return CheckEnum(card.set);
        }
    }
}
