using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardTypeFilter : AEnumNumberFilter<CardTypes>
    {
        public override bool Check(Card card)
        {
            return CheckEnum(card.type);
        }
    }
}
