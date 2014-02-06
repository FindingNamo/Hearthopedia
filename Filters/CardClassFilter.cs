using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardClassFilter : AEnumNumberFilter<CardClass>
    {
        private string _name = "Class";

        private CardClassFilter()
        {
            _FilterOptions = EnumUtilities.AssembleEnumerableOptionList<CardClass>();
        }

        private static CardClassFilter _instance;
        public static CardClassFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new CardClassFilter());
            }
        }
        public override bool Check(Card card)
        {
            return CheckEnum(card.classs);
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
