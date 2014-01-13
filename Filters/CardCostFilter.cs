using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardCostFilter : ANumberTextboxFilter
    {
        public override bool Check(Card card)
        {
            return CheckInt(card.cost);
        }
    }
}
