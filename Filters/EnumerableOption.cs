using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class EnumerableOption<TEnum>
    {
        public string Name { get; set; }
        public TEnum EnumValue { get; set; }
        public bool Value { get; set; }
    }
}
