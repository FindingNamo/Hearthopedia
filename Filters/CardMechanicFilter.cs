using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardMechanicFilter : AEnumNumberFilter<CardMechanic>
    {
        private CardMechanicFilter()
        {
            _FilterOptions = EnumUtilities.AssembleEnumerableOptionList<CardMechanic>();
        }

        private static CardMechanicFilter _instance;
        public static CardMechanicFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new CardMechanicFilter());
            }
        }

        public override bool Check(Card card)
        {
            foreach(Mechanic mechanic in card.MechanicData)
            {
                if (CheckEnum(mechanic.Id))
                    return true;
            }
                
            return false;
        }
    }
}
