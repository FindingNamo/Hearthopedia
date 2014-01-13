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
    public abstract class ANumberFilter : ICardFilter
    {
        /// <summary>
        /// The supported operators for number typed filters.
        /// </summary>
        public List<string> SupportedOperators
        {
            get
            {
                return new List<string>()
                    {
                        "less than",
                        "less than or equal to",
                        "equal to",
                        "greater than or equal to",
                        "greater than",
                        "not equal to ",
                     };
            }
        }

        protected enum NumberFilterOperation
        {
            LessThan,
            LessThanEqual,
            Equal,
            GreaterThanEqual,
            GreaterThan,
            NotEqual,
        }

        /// <summary>
        /// The operation the user has chosen.
        /// </summary>
        protected NumberFilterOperation ChosenOperation;

        /// <summary>
        /// The value the user has chosen.
        /// </summary>
        protected int ChosenValue;

        /// <summary>
        /// Checks to see if the card passes this test.
        /// </summary>
        public abstract bool Check(Card card);

        /// <summary>
        /// Simply call this with your number.
        /// </summary>
        /// <returns></returns>
        protected bool CheckInt(int i)
        {
            switch(ChosenOperation)
            {
                case NumberFilterOperation.Equal:
                    return i == ChosenValue;
                case NumberFilterOperation.GreaterThan:
                    return i > ChosenValue;
                case NumberFilterOperation.GreaterThanEqual:
                    return i >= ChosenValue;
                case NumberFilterOperation.LessThan:
                    return i < ChosenValue;
                case NumberFilterOperation.LessThanEqual:
                    return i <= ChosenValue;
                case NumberFilterOperation.NotEqual:
                    return i != ChosenValue;

                default:
                    // Didn't set a valud operation?
                    throw new ArgumentException();
            }
        }

        public abstract void Dispose();
        public abstract FrameworkElement GetFilterValueXamlElement();
        public abstract void SetOperationIndex(int selectedIndex);
    }
}
