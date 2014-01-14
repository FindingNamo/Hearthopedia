using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hearthopedia.Filters
{
    public interface ICardFilter
    {
        bool Check(Card card);
    }
}
