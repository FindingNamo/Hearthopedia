using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardQualityFilter : AEnumNumberFilter<CardQuality>
    {
        private string _name = "Quality";

        public string Name
        {
            get
            {
                return _name;
            }
        }

        private CardQualityFilter()
        {
            _FilterOptions = EnumUtilities.AssembleEnumerableOptionList<CardQuality>();
        }

        private static CardQualityFilter _instance;
        public static CardQualityFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new CardQualityFilter());
            }
        }

        public override bool Check(Card card)
        {
            return CheckEnum(card.quality);
        }
    }
}
