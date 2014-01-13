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
    public abstract class ANumberTextboxFilter : ANumberFilter
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
        /// The textbox the user has typed into.
        /// </summary>
        private TextBox _filterTextBox;

        /// <summary>
        /// Gets a textbox to type in.
        /// </summary>
        public override FrameworkElement GetFilterValueXamlElement()
        {
            return _filterTextBox ?? (_filterTextBox = GenerateTextBox());
        }

        /// <summary>
        /// Setup and create the special textbox
        /// </summary>
        private TextBox GenerateTextBox()
        {
            //TODO format this for numbers only?
            TextBox filterBox = new TextBox();
            filterBox.TextChanged += OnValueChange;
            return filterBox;
        }

        private void OnValueChange(object sender, TextChangedEventArgs args)
        {
            // TODO: SafeGuard this.
            ChosenValue = int.Parse(((TextBox)sender).Text);
        }

        /// <summary>
        /// Sets the operation index
        /// </summary>
        public override void SetOperationIndex(int selectedIndex)
        {
            ChosenOperation = (NumberFilterOperation) selectedIndex;
        }

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

        /// <summary>
        /// Unregister any events.
        /// </summary>
        public override void Dispose()
        {
            _filterTextBox.TextChanged -= OnValueChange;
        }
    }
}
