using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hearthopedia.Filters
{
    public abstract class AEnumNumberFilter<EnumType> : ICardFilter where EnumType : struct
    {
        protected List<EnumerableOption<EnumType>> _FilterOptions;
        public List<EnumerableOption<EnumType>> FilterOptions
        {
            get
            {
                return _FilterOptions;
            }
        }

        /// <summary>
        /// Simply call this with your number.
        /// </summary>
        /// <returns></returns>
        protected bool CheckEnum(int enumValue)
        {
            foreach (EnumerableOption<EnumType> enumOption in _FilterOptions)
            {
                // This is really bad... but hey, they're all ints... right?
                if (enumOption.Value && enumValue == (int)(object)enumOption.EnumValue)
                    return true;

            }

            return false;
        }

        public abstract bool Check(Card card);

        public void Disable()
        {
            foreach (EnumerableOption<EnumType> option in _FilterOptions)
            {
                option.Value = false;
            }
        }

    }
}
